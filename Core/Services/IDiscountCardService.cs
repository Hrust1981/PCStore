using Core.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services
{
    public interface IDiscountCardService
    {
        void AddDiscountCard(Buyer buyer, int totalPurchaseAmount = 0);
        void AddCheerfulDiscountCard(Buyer buyer);
        void SetDayForIssueQuantumDiscountCard(int amountDays);
        string SetDateIssueForQuantumDiscountCard(IServiceCollection services);
        string SetWorkDatesForCheerfulDiscountCard(int upperRangeLimitInDays);
    }
}
