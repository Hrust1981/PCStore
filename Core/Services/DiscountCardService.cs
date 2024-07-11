using Core.Entities;
using System.Globalization;

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
            var dateIssueQuantumDiscountCard = DateTime.Parse(GetValueFromJson("DateIssue"));
            var numberDaysUntilCloseQuantumDiscountCard = int.Parse(GetValueFromJson("AmountDays"));

            if (discountCards.Any(dc => dc.Name == "QuantumDiscountCard"))
            {
                if (DateTime.Now.Date >= dateIssueQuantumDiscountCard.AddDays(numberDaysUntilCloseQuantumDiscountCard))
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

        private DateOnly GenerateDate(int upperRangeLimitInDays = 0)
        {
            int day;
            var dayTimeNow = DateTime.Now;

            Random random = new Random();
            day = random.Next(dayTimeNow.Day + 1, DateTime.DaysInMonth(dayTimeNow.Year, dayTimeNow.Month) - upperRangeLimitInDays);

            return string.Equals(CultureInfo.CurrentCulture.Name, "ru-RU") ? 
                DateOnly.Parse($"{day}.{dayTimeNow.Month}.{dayTimeNow.Year}") :
                DateOnly.Parse($"{dayTimeNow.Year}.{dayTimeNow.Month}.{day}");
        }

        public void SetDayForIssueQuantumDiscountCard(int amountDays)
        {
            ChangeValueInJson(amountDays, "AmountDays");
        }

        public DateOnly GenerateDateIssueOrWorkDiscountCard(string nameReplacableElement, int upperRangeLimitInDays = 0)
        {
            var date = GenerateDate(upperRangeLimitInDays);
            ChangeValueInJson(date, nameReplacableElement);
            return date;
        }

        private void ChangeValueInJson(System.Object item, string nameReplacableElement)
        {
            var valueJson = GetValueFromJson(nameReplacableElement);
            var path = CustomConfigurationManager.GetValueByKey("PathToSettingsForIssueDiscountCards");
            
            var stream = GetStream();
            var newDate = stream.Replace(valueJson, item.ToString());

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(newDate);
            }
        }

        public string GetValueFromJson(string nameReplacableElement)
        {
            var stream = GetStream();
            var index = stream.IndexOf(nameReplacableElement) + nameReplacableElement.Length + 3;

            var count = 0;
            var indexString = index;
            while (stream[indexString] != '"' && stream[indexString] != ',' && stream[indexString] != '}')
            {
                indexString++;
                count++;
            }

            return stream.Substring(index, count);
        }

        private string GetStream()
        {
            var path = CustomConfigurationManager.GetValueByKey("PathToSettingsForIssueDiscountCards");
            using (StreamReader reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
