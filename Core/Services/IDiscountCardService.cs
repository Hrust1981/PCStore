using Core.Entities;

namespace Core.Services
{
    public interface IDiscountCardService
    {
        void AddDiscountCards(Buyer buyer);
        int CalculateTotalAmount(Buyer buyer, out int totalAmountWithDiscount);
    }
}
