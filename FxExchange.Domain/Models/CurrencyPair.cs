namespace FxExchange.Domain.Models
{
    public class CurrencyPair
    {
        public decimal BaseAmount { get; set; }
        public string BaseCurrency { get; set; }
        public decimal QuoteAmount { get; set; }
        public string QuoteCurrency { get; set; }
    }
}
