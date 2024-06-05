namespace Core.Entities
{
    public abstract class DiscountCard
    {
        public abstract int TotalPurchaseAmount { get; set; }

        public abstract int Discount { get; }
    }
}
