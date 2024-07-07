namespace Core.Entities
{
    public class CyclicDiscountCard : DiscountCard
    {
        private int _discount;

        public CyclicDiscountCard()
        {
            Name = "CyclicDiscountCard";
        }

        public CyclicDiscountCard(int discount, int totalPurchaseAmount)
        {
            Name = "CyclicDiscountCard";
            _discount = discount;
            TotalPurchaseAmount = totalPurchaseAmount;
        }

        public override int Discount => _discount;
        public int TotalPurchaseAmount { get; set; }
    }
}
