using Core.Entities;

namespace Core.Services
{
    public class DiscountCardService : IDiscountCardService
    {
        public int CalculateTotalAmount(Buyer buyer, out int totalAmountWithDiscount)
        {
            var discountCard = buyer.DiscountCards?.OrderByDescending(d => d.Discount).FirstOrDefault();
            var totalAmount = buyer.ShoppingCart.Sum(s => s.Price);
            
            totalAmountWithDiscount = discountCard == null ? 
                0 :
                totalAmountWithDiscount = totalAmount - totalAmount * discountCard.Discount / 100;

            return totalAmount;
        }

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
