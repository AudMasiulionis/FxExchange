using FxExchange.Domain.Models;

namespace FxExchange.Domain
{
    public interface ICurrencyPairRepository
    {
        Money GetByBaseCurrency(string iso);
    }
}
