using Core.Entities;

namespace Core.Services
{
    public interface IDiscountCardService
    {
        void AddDiscountCard(Buyer buyer, int totalPurchaseAmount = 0);
        public DateOnly GenerateDate();
        public void SetDayForIssueQuantumDiscountCard(string date);
        public DateOnly GenerateDateIssueQuantumDiscountCard();
    }
}
