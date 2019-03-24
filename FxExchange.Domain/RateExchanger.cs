using System;
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
            AssertParameters(baseCurrencyISO, quoteCurrencyISO, amountExchanged);

            var baseRate = this.exchangeRateRepository.GetByBaseCurrency(baseCurrencyISO);
            var quoteRate = this.exchangeRateRepository.GetByBaseCurrency(quoteCurrencyISO);
            if (baseRate == null || quoteRate == null)
            {
                throw new RateNotFoundException();
            }

            var result = CalculateRate(baseRate.Amount, quoteRate.Amount, amountExchanged);
            return new Money(result, baseCurrencyISO);
        }

        private static decimal CalculateRate(decimal baseAmount, decimal qouteAmount, decimal amountExchanged)
        {
            var result = decimal.Divide(baseAmount, qouteAmount);
            return decimal.Multiply(result, amountExchanged);
        }

        private void AssertParameters(string baseCurrencyISO, string quoteCurrencyISO, decimal amountExchanged)
        {
            if (string.IsNullOrEmpty(baseCurrencyISO))
            {
                throw new ArgumentException(nameof(baseCurrencyISO));
            }
            if (string.IsNullOrEmpty(quoteCurrencyISO))
            {
                throw new ArgumentException(nameof(quoteCurrencyISO));
            }
            if (amountExchanged <= 0)
            {
                throw new ArgumentException(nameof(amountExchanged));
            }
        }
    }
}
