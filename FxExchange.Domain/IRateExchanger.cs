using FxExchange.Domain.Models;

namespace FxExchange.Domain
{
    public interface IRateExchanger
    {
        Money Exchage(string baseCurrencyISO, string quoteCurrencyISO, decimal amountExchanged);
    }
}