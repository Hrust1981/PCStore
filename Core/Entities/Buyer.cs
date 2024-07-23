using Core.Enumerations;

namespace Core.Entities
{
    public class Buyer : User
    {
        // For testing 
        public Buyer(Guid id, string name, string login, string password, string email, Role role)
            : base(name, login, password, email, role)
        {
            DiscountCards = new List<DiscountCard>();
        }

        public Buyer(string name, string login, string password, string email, Role role)
            : base(name, login, password, email, role)
        {
            DiscountCards = new List<DiscountCard>();
        }

        public bool IsAuthenticated { get; set; }
        public List<DiscountCard> DiscountCards { get; set; }
        public int TotalPurchaseAmount { get; set; }
    }
}
