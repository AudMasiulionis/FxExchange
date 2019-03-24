using FxExchange.Domain;
using FxExchange.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace FxExchange.Infrastructure
{
    public class HardCodedCurrencyPairRepository : ICurrencyPairRepository
    {
        private static List<Money> _rates = new List<Money>
        {
            new Money(743.94m,"EUR"),
            new Money(663.11m, "USD"),
            new Money(852.85m, "GBP"),
            new Money(76.10m, "SEK"),
            new Money(78.40m, "NOK"),
            new Money(683.58m, "CHF"),
            new Money(5.9740m, "JPY"),
            new Money(100m, "DKK")
        };

        public Money GetByBaseCurrency(string iso)
        {
            return _rates.FirstOrDefault(r => r.Currency == iso);
        }
    }
}
