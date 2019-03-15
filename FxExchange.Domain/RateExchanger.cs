using FxExchange.Domain.Exceptions;
using FxExchange.Domain.Models;

namespace FxExchange.Domain
{
    public class RateExchanger : IRateExchanger
    {
        private readonly ICurrencyPairRepository exchangeRateRepository;

        public RateExchanger(ICurrencyPairRepository exchangeRateRepository)
        {
            this.exchangeRateRepository = exchangeRateRepository;
        }

        public Money Exchage(string baseCurrencyISO, string quoteCurrencyISO, decimal amountExchanged)
        {
            if (amountExchanged == 0)
            {
                return new Money(0, baseCurrencyISO);
            }

            var baseRate = this.exchangeRateRepository.GetByBaseCurrency(baseCurrencyISO);
            if (baseRate != null)
            {
                decimal exchangeResult = 0m;
                if (baseRate.QuoteCurrency == quoteCurrencyISO)
                {
                    exchangeResult = CalculateRate(baseRate.QuoteAmount, baseRate.BaseAmount, amountExchanged);
                }
                else
                {
                    CurrencyPair quoteRate = this.exchangeRateRepository.GetByBaseCurrency(quoteCurrencyISO);
                    if (quoteRate == null)
                    {
                        throw new RateNotFoundException($"Rate for currency pair{baseCurrencyISO}/{quoteCurrencyISO} not found");
                    }
                    exchangeResult = CalculateRate(baseRate.QuoteAmount, quoteRate.QuoteAmount, amountExchanged);
                }

                return new Money(exchangeResult, baseCurrencyISO);
            }

            baseRate = this.exchangeRateRepository.GetByQuoteCurrency(baseCurrencyISO);
            if (baseRate != null && baseRate.BaseCurrency == quoteCurrencyISO)
            {
                decimal exchangeResult = CalculateRate(baseRate.BaseAmount, baseRate.QuoteAmount, amountExchanged);
                return new Money(exchangeResult, baseCurrencyISO);
            }

            throw new RateNotFoundException($"Rate for currency pair{baseCurrencyISO}/{quoteCurrencyISO} not found");
        }

        private static decimal CalculateRate(decimal baseAmount, decimal qouteAmount, decimal amountExchanged)
        {
            var result = decimal.Divide(baseAmount, qouteAmount);
            return decimal.Multiply(result, amountExchanged);
        }
    }
}
