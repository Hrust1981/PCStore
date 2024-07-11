namespace Core.Entities
{
    public class CheerfulDiscountCard : DiscountCard
    {
        private int _discount = 10;

        public CheerfulDiscountCard()
        {
            Name = "CheerfulDiscountCard";
        }

        public override int Discount => _discount;
    }
}
