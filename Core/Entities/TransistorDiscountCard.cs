namespace Core.Entities
{
    public class TransistorDiscountCard : DiscountCard
    {
        private int _totalPurchaseAmount;
        private int _discount = 10;
        public override int TotalPurchaseAmount { get => _totalPurchaseAmount; set => _totalPurchaseAmount += value; }

        public override int Discount => _discount;
    }
}
