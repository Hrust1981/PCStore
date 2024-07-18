using Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Core.Services
{
    public class DiscountCardService : IDiscountCardService
    {
        private readonly Random _random;
        private readonly IOptionsMonitor<DiscountCardsOptions> _setupOptions;

        public DiscountCardService(IOptionsMonitor<DiscountCardsOptions> setupOptions)
        {
            _random = new Random();
            _setupOptions = setupOptions;
        }

        public void AddDiscountCard(Buyer buyer, int totalAmount = 0)
        {
            var discountCards = buyer.DiscountCards;
            var totalPurchaseAmount = buyer.TotalPurchaseAmount;
            var dateIssueQuantumDiscountCard = DateTime.Parse(_setupOptions.CurrentValue.DateIssue);
            var numberDaysUntilCloseQuantumDiscountCard = _setupOptions.CurrentValue.AmountDays;

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

        public void AddCheerfulDiscountCard(Buyer buyer)
        {
            var stringRepresentationDate = _setupOptions.CurrentValue.WorkDates;
            var date = DateTime.Parse(stringRepresentationDate);
            var numberDays = _setupOptions.CurrentValue.NumberDays;
            var discountValue =  DateTime.Today >= date && DateTime.Today <= date.AddDays(numberDays) ? 10 : 0;
            buyer.DiscountCards.Add(new CheerfulDiscountCard(discountValue));
        }

        private string GenerateDate(int upperRangeLimitInDays = 0)
        {
            int day;
            var dayTimeNow = DateTime.Now;

            Random random = new Random();
            day = random.Next(dayTimeNow.Day + 1, DateTime.DaysInMonth(dayTimeNow.Year, dayTimeNow.Month) -
                upperRangeLimitInDays);

            return string.Equals(CultureInfo.CurrentCulture.Name, "ru-RU", StringComparison.InvariantCultureIgnoreCase) ? 
                $"{day}.{dayTimeNow.Month}.{dayTimeNow.Year}" :
                $"{dayTimeNow.Year}.{dayTimeNow.Month}.{day}";
        }

        public void SetDayForIssueQuantumDiscountCard(int amountDays)
        {
            _setupOptions.CurrentValue.AmountDays = amountDays;
        }

        public string SetDateIssueForQuantumDiscountCard(IServiceCollection services)
        {
            var date = GenerateDate();
            _setupOptions.CurrentValue.DateIssue = date;
            services.Configure<DiscountCardsOptions>(x => x.DateIssue = date);
            return date;
        }

        public string SetWorkDatesForCheerfulDiscountCard(int upperRangeLimitInDays)
        {
            var date = GenerateDate();
            _setupOptions.CurrentValue.WorkDates = date;
            return date;
        }
    }
}
