namespace Core
{
    public class DiscountCardsOptions
    {
        public const string ConfigKeyQuantumDC = "SETUP_OPTIONS_DC:SettingsForQuantumDC";
        public const string ConfigKeyCheerfulDC = "SETUP_OPTIONS_DC:SettingsForCheerfulDC";

        // Settings for issuance for Quantum discount cards
        public int AmountDays {  get; set; }
        public string DateIssue {  get; set; }

        // Settings for issuance for Cheerful discount cards
        public string WorkDates { get; set; }
        public int NumberDays { get; set; }
    }
}
