using Core.Services;

namespace Core.Entities
{
    public class CheerfulDiscountCard : DiscountCard
    {
        private readonly IDiscountCardService _discountCardService;
        private static int _discount;

        private CheerfulDiscountCard()
        {
            Name = "CheerfulDiscountCard";
            _discountCardService = new DiscountCardService();
        }

        public override int Discount => _discount;

        public static async Task<CheerfulDiscountCard> CreateAsync()
        {
            var instance = new CheerfulDiscountCard();
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
