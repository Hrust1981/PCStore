namespace Core.Entities
{
    public class CyclicDiscountCard : DiscountCard
    {
        private int _discount = 5;
        public override int Discount => _discount;
    }
}
