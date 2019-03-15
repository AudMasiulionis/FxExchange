using System;
using System.Text.RegularExpressions;

namespace FxExchange.CLI
{
    public class ExchangeCommand
    {
        public ExchangeCommand(string commandString)
        {
            ParseCommand(commandString);
        }

        public ExchangeCommand(string BaseCurrencyISO, string quoteCurrencyISO, decimal amount)
        {
            this.BaseCurrencyISO = BaseCurrencyISO;
            this.QuoteCurrencyISO = quoteCurrencyISO;
            this.Amount = amount;
        }

        public string BaseCurrencyISO { get; set; }
        public string QuoteCurrencyISO { get; set; }
        public decimal Amount { get; set; }

        private void ParseCommand(string commandString)
        {
            if (!Regex.IsMatch(commandString, @"^[A-Z]{3}\/[A-Z]{3}[\s](\d*|\d*([.,])\d*)$"))
            {
                throw new FormatException($"Command=[{commandString}] is invalid format");
            }

            var splittedCommand = commandString.Split(' ');
            var currencyPair = splittedCommand[0];
            var currencies = currencyPair.Split('/');
            this.BaseCurrencyISO = currencies[0];
            this.QuoteCurrencyISO = currencies[1];
            this.Amount = decimal.Parse(splittedCommand[1]);
        }

        public override bool Equals(object obj)
        {
            var command = obj as ExchangeCommand;
            return command != null &&
                   BaseCurrencyISO == command.BaseCurrencyISO &&
                   QuoteCurrencyISO == command.QuoteCurrencyISO &&
                   Amount == command.Amount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BaseCurrencyISO, QuoteCurrencyISO, Amount);
        }
    }
}
