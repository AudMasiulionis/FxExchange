using FxExchange.Domain;
using FxExchange.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace FxExchange.Infrastructure
{
    public class HardCodedCurrencyPairRepository : ICurrencyPairRepository
    {
        private static List<CurrencyPair> _rates = new List<CurrencyPair>
        {
            new CurrencyPair
            {
                    BaseCurrency = "EUR",
                    BaseAmount = 100m,
                    QuoteCurrency = "DKK",
                    QuoteAmount = 743.94m
            },
            new CurrencyPair
            {
                    BaseCurrency = "USD",
                    BaseAmount = 100m,
                    QuoteCurrency = "DKK",
                    QuoteAmount = 663.11m
            },
            new CurrencyPair
            {
                    BaseCurrency = "GBP",
                    BaseAmount = 100m,
                    QuoteCurrency = "DKK",
                    QuoteAmount = 852.85m
            },
            new CurrencyPair
            {
                    BaseCurrency = "SEK",
                    BaseAmount = 100m,
                    QuoteCurrency = "DKK",
                    QuoteAmount = 76.10m
            },
            new CurrencyPair
            {
                    BaseCurrency = "NOK",
                    BaseAmount = 100m,
                    QuoteCurrency = "DKK",
                    QuoteAmount = 78.40m
            },
            new CurrencyPair
            {
                    BaseCurrency = "CHF",
                    BaseAmount = 100m,
                    QuoteCurrency = "DKK",
                    QuoteAmount = 683.58m
            },
            new CurrencyPair
            {
                    BaseCurrency = "JPY",
                    BaseAmount = 100m,
                    QuoteCurrency = "DKK",
                    QuoteAmount = 5.9740m
            }
        };

        public CurrencyPair GetByBaseCurrency(string iso)
        {
            return _rates.FirstOrDefault(r => r.BaseCurrency == iso);
        }

        public CurrencyPair GetByQuoteCurrency(string iso)
        {
            return _rates.FirstOrDefault(r => r.QuoteCurrency == iso);
        }
    }
}
