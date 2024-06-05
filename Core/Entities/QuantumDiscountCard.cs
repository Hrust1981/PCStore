namespace Core.Entities
{
    public class QuantumDiscountCard : DiscountCard
    {
        private int _totalPurchaseAmount;
        private int _discount = 20;
        public override int TotalPurchaseAmount { get => _totalPurchaseAmount; set => _totalPurchaseAmount += value; }

        public override int Discount => _discount;
    }
}
