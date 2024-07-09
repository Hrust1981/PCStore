using Core.Entities;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Transactions;

namespace Core.Services
{
    public class DiscountCardService : IDiscountCardService
    {
        private readonly static Random _random;

        static DiscountCardService()
        {
            _random = new Random();
        }

        public void AddDiscountCard(Buyer buyer, int totalAmount = 0)
        {
            var discountCards = buyer.DiscountCards;
            var totalPurchaseAmount = buyer.TotalPurchaseAmount;
            var dateIssueQuantumDiscountCard = DateTime.Parse("01.07.2024");

            if (discountCards.Any(dc => dc.Name == "QuantumDiscountCard"))
            {
                if (DateTime.Now.Date >= dateIssueQuantumDiscountCard.AddDays(1))
                {
                    discountCards.Clear();
                    discountCards.Add(new CyclicDiscountCard(5, 5000));
                }
                return;
            }

            if (discountCards.Any(dc => dc.Name == "CyclicDiscountCard"))
            {
                var cyclicDiscountCard = discountCards.FirstOrDefault() as CyclicDiscountCard;
                var total = cyclicDiscountCard?.TotalPurchaseAmount;
                var discount = cyclicDiscountCard?.Discount;

                switch (total)
                {
                    case > 5000 and <= 12500:
                        discount = 10;
                        break;
                    case > 12500 and <= 25000:
                        discount = 15;
                        break;
                    case > 25000 and >= 50000:
                        discount = 5;
                        total = 5000;
                        break;
                }
            }
            else if (DateTime.Now.Date == dateIssueQuantumDiscountCard && _random.Next(2) == 1)
            {
                if (discountCards.Any())
                {
                    discountCards.Clear();
                }
                discountCards.Add(new QuantumDiscountCard());
            }
            else if (totalPurchaseAmount >= 5000 && totalPurchaseAmount < 12500)
            {
                discountCards.Add(new TubeDiscountCard());
            }
            else if (totalPurchaseAmount >= 12500 && totalPurchaseAmount < 25000)
            {
                discountCards.Add(new TransistorDiscountCard());
            }
            else if (totalPurchaseAmount >= 25000)
            {
                discountCards.Add(new IntegratedDiscountCard());
            }
        }

        public DateOnly GenerateDate()
        {
            int day;
            var dayTimeNow = DateTime.Now;

            Random random = new Random();
            day = random.Next(dayTimeNow.Day + 1, DateTime.DaysInMonth(dayTimeNow.Year, dayTimeNow.Month));

            return string.Equals(CultureInfo.CurrentCulture.Name, "ru-RU") ? 
                DateOnly.Parse($"{day}.{dayTimeNow.Month}.{dayTimeNow.Year}") :
                DateOnly.Parse($"{dayTimeNow.Year}.{dayTimeNow.Month}.{day}");
        }

        public void SetDayForIssueQuantumDiscountCard(string date)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["DayIssueForQuantumDiscountCard"].Value = date;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        public DateOnly GenerateDateIssueQuantumDiscountCard()
        {
            var date = GenerateDate();
            var path = CustomConfigurationManager.GetValueByKey("PathToSettingsForIssueDiscountCards");

            DateIssueQuantumDiscountCard dateIssue =
                new DateIssueQuantumDiscountCard("DateIssueForQuantumDiscountCard", date);

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(fs, dateIssue);
            }
            return date;
        }
    }
}
