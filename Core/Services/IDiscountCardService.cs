using Core.Entities;

namespace Core.Services
{
    public interface IDiscountCardService
    {
        Task AddDiscountCardAsync(Buyer buyer, int totalPurchaseAmount = 0);
        Task SetDayForIssueQuantumDiscountCardAsync(int amountDays);
        Task<DateOnly> GenerateDateIssueOrWorkDiscountCardAsync(string nameReplacableElement, int upperRangeLimitInDays = 0);
        Task<string> GetValueFromJsonAsync(string nameReplacableElement);
    }
}
