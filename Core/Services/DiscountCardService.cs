using Core.Entities;

namespace Core.Services
{
    public class DiscountCardService : IDiscountCardService
    {
        public void AddDiscountCard(Buyer buyer)
        {
            if (buyer.TotalPurchaseAmount >= 5000 && buyer.TotalPurchaseAmount < 12500)
            {
                buyer.DiscountCards.Add(new TubeDiscountCard());
            }
            else if (buyer.TotalPurchaseAmount >= 12500 && buyer.TotalPurchaseAmount < 25000)
            {
                buyer.DiscountCards.Add(new TransistorDiscountCard());
            }
            else if (buyer.TotalPurchaseAmount >= 25000)
            {
                buyer.DiscountCards.Add(new IntegratedDiscountCard());
            }
        }
    }
}
