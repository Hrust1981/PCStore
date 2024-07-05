namespace Core.Entities
{
    public class QuantumDiscountCard : DiscountCard
    {
        private int _discount = 20;

        public QuantumDiscountCard()
        {
            Name = "QuantumDiscountCard";
        }

        public override int Discount => _discount;
    }
}
