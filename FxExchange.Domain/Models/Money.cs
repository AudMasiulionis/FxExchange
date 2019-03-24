namespace FxExchange.Domain.Models
{
    public class Money
    {
        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; }
        public string Currency { get; }
    }
}
