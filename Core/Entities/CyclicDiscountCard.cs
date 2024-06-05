namespace Core.Entities
{
    public class CyclicDiscountCard : DiscountCard
    {
        private int _totalPurchaseAmount;
        private int _discount = 5;
        public override int TotalPurchaseAmount { get => _totalPurchaseAmount; set => _totalPurchaseAmount += value; }

        public override int Discount => _discount;
    }
}
