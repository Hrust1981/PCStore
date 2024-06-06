using Core.Entities;

namespace Core.Services
{
    public interface IDiscountCardService
    {
        void AddDiscountCard(Buyer buyer);
        int CalculateTotalAmount(Buyer buyer, out int totalAmountWithDiscount);
    }
}
