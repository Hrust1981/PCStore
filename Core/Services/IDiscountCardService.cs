using Core.Entities;

namespace Core.Services
{
    public interface IDiscountCardService
    {
        void AddDiscountCard(Buyer buyer, int totalPurchaseAmount = 0);
        public DateOnly GenerateDate();
        public void SetDayForIssueQuantumDiscountCard(int amountDays);
        public DateOnly GenerateDateIssueQuantumDiscountCard();
    }
}
