using FxExchange.Domain.Models;

namespace FxExchange.Domain
{
    public interface ICurrencyPairRepository
    {
        CurrencyPair GetByBaseCurrency(string iso);
        CurrencyPair GetByQuoteCurrency(string iso);
    }
}
