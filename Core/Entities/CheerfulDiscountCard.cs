using Core.Services;

namespace Core.Entities
{
    public class CheerfulDiscountCard : DiscountCard
    {
        private readonly int _discount;

        public CheerfulDiscountCard()
        {
            Name = "CheerfulDiscountCard";
            _discount = GetDiscount();
        }

        public override int Discount => _discount;

        private static int GetDiscount()
        {
            IDiscountCardService discountCardService = new DiscountCardService();

            var stringRepresentationDate = discountCardService.GetValueFromJson("WorkDatesCheerfulDiscountCard");
            var date = DateTime.Parse(stringRepresentationDate);

            return DateTime.Today >= date && DateTime.Today <= date.AddDays(10) ? 10 : 0;
        }
    }
}
