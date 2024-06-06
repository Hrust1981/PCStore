namespace Core.Entities
{
    public class TubeDiscountCard : DiscountCard
    {
        private int _discount = 5;
        public override int Discount => _discount;
    }
}
