namespace Core.Entities
{
    public class DiscountCard
    {
        public int PurchaseAmount { get; set; }
        private readonly int _discount;

        public DiscountCard(int discount)
        {
            _discount = discount;
        }

        public void CalculationOfAccumulativePart(int purchaseAmount)
        {
            PurchaseAmount += purchaseAmount;
        }
    }
}
