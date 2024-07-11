using Core.Entities;

namespace Core.Services
{
    public interface IDiscountCardService
    {
        void AddDiscountCard(Buyer buyer, int totalPurchaseAmount = 0);
        public void SetDayForIssueQuantumDiscountCard(int amountDays);
        public DateOnly GenerateDateIssueOrWorkDiscountCard(string nameReplacableElement, int upperRangeLimitInDays = 0);
        public string GetValueFromJson(string nameReplacableElement);
    }
}
