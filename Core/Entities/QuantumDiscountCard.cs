namespace Core.Entities
{
    public class QuantumDiscountCard : DiscountCard
    {
        private int _discount = 20;
        public override int Discount => _discount;
    }
}
