namespace Core
{
    public class DateIssueQuantumDiscountCard
    {
        public DateIssueQuantumDiscountCard(string key, DateOnly value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }
        public DateOnly Value { get; set; }
    }
}
