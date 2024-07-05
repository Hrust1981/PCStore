using Core.Entities;

namespace Core.Services
{
    public class DiscountCardService : IDiscountCardService
    {
        private readonly static Random _random;

        static DiscountCardService()
        {
            _random = new Random();
        }

        public void AddDiscountCard(Buyer buyer)
        {
            var discountCards = buyer.DiscountCards;
            var totalPurchaseAmount = buyer.TotalPurchaseAmount;

            if (discountCards.Any(dc => dc.Name == "QuantumDiscountCard"))
            {
                return;
            }

            if (DateTime.Now.Date == DateTime.Parse("05.07.2024") && GetRandomBooleanValue())
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

        private bool GetRandomBooleanValue()
        {
            return _random.Next(2) == 1;
        }
    }
}
