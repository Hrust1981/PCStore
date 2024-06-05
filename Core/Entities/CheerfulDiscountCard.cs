namespace Core.Entities
{
    public class CheerfulDiscountCard : DiscountCard
    {
        private int _totalPurchaseAmount;
        private int _discount = 10;
        public override int TotalPurchaseAmount { get => _totalPurchaseAmount; set => _totalPurchaseAmount += value; }

        public override int Discount => _discount;
    }
}
