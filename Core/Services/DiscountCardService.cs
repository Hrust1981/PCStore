using Core.Entities;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Core.Services
{
    public class DiscountCardService : IDiscountCardService
    {
        private readonly Random _random;
        private readonly IOptionsMonitor<DiscountCardService> _optionsMonitor;

        public DiscountCardService(IOptionsMonitor<DiscountCardService> optionsMonitor)
        {
            _random = new Random();
            _optionsMonitor = optionsMonitor;
        }

        public async Task AddDiscountCardAsync(Buyer buyer, int totalAmount = 0)
        {
            var discountCards = buyer.DiscountCards;
            var totalPurchaseAmount = buyer.TotalPurchaseAmount;
            var dateIssueQuantumDiscountCard = DateTime.Parse(await GetValueFromJsonAsync("DateIssue"));
            var numberDaysUntilCloseQuantumDiscountCard = int.Parse(await GetValueFromJsonAsync("AmountDays"));

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
                if (discountCards.Count != 0)
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
            day = random.Next(dayTimeNow.Day + 1, DateTime.DaysInMonth(dayTimeNow.Year, dayTimeNow.Month) -
                upperRangeLimitInDays);

            return string.Equals(CultureInfo.CurrentCulture.Name, "ru-RU", StringComparison.InvariantCultureIgnoreCase) ? 
                DateOnly.Parse($"{day}.{dayTimeNow.Month}.{dayTimeNow.Year}") :
                DateOnly.Parse($"{dayTimeNow.Year}.{dayTimeNow.Month}.{day}");
        }

        public async Task SetDayForIssueQuantumDiscountCardAsync(int amountDays)
        {
            await ChangeValueInJsonAsync(amountDays, "AmountDays");
        }

        public async Task<DateOnly> GenerateDateIssueOrWorkDiscountCardAsync(string nameReplacableElement, int upperRangeLimitInDays = 0)
        {
            var date = GenerateDate(upperRangeLimitInDays);
            await ChangeValueInJsonAsync(date, nameReplacableElement);
            return date;
        }

        private async Task ChangeValueInJsonAsync(System.Object item, string nameReplacableElement)
        {
            var valueJson = await GetValueFromJsonAsync(nameReplacableElement);
            var path = CustomConfigurationManager.GetValueByKey("PathToSettingsForIssueDiscountCards");
            
            var stream = await GetStreamAsync();
            var newDate = stream.Replace(valueJson, item.ToString());

            using StreamWriter writer = new StreamWriter(path, false);
            await writer.WriteAsync(newDate);
        }

        public async Task<string> GetValueFromJsonAsync(string nameReplacableElement)
        {
            var stream = await GetStreamAsync();
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

        private async Task<string> GetStreamAsync()
        {
            //var path = CustomConfigurationManager.GetValueByKey("PathToSettingsForIssueDiscountCards");
            var path = _optionsMonitor.Get("PathToSettingsForIssueDiscountCards").;
            using StreamReader reader = new StreamReader(path);
            return await reader.ReadToEndAsync();
        }
    }
}
