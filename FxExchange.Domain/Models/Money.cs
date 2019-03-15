using System;

namespace FxExchange.Domain.Models
{
    public class Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public override bool Equals(object obj)
        {
            var money = obj as Money;
            return money != null &&
                   Amount == money.Amount &&
                   Currency == money.Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
    }
}
