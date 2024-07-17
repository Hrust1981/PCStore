using Core.Services;

namespace Core.Entities
{
    public class CheerfulDiscountCard : DiscountCard
    {
        private readonly IDiscountCardService _discountCardService;
        private int _discount;

        public CheerfulDiscountCard(IDiscountCardService discountCardService)
        {
            Name = "CheerfulDiscountCard";
            _discountCardService = discountCardService;
        }

        public override int Discount => _discount;

        public async Task<CheerfulDiscountCard> CreateAsync()
        {
            var instance = new CheerfulDiscountCard(_discountCardService);
            _discount = await instance.GetDiscountAsync();
            return instance;
        }


        private async Task<int> GetDiscountAsync()
        {
            var stringRepresentationDate = await _discountCardService.GetValueFromJsonAsync("WorkDatesCheerfulDiscountCard");
            var date = DateTime.Parse(stringRepresentationDate);
            var numberDays = await _discountCardService.GetValueFromJsonAsync("NumberDaysCheerfulDiscountCardActive");
            return DateTime.Today >= date && DateTime.Today <= date.AddDays(int.Parse(numberDays)) ? 10 : 0;
        }
    }
}
