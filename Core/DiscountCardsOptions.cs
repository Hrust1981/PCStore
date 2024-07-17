namespace Core
{
    public class DiscountCardsOptions
    {
        // Settings for issuance for Quantum discount cards
        public int AmountDays {  get; set; }
        public string DateIssue {  get; set; }

        // Settings for issuance for Cheerful discount cards
        public string WorkDates { get; set; }
        public int NumberDays { get; set; }
    }
}
