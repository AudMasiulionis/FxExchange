using FxExchange.Domain;
using FxExchange.Domain.Exceptions;
using FxExchange.Infrastructure;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace FxExchange.CLI
{
    [HelpOption]
    public class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, Description = "Currency pair")]
        [RegularExpression(@"^[A-Z]{3}\/[A-Z]{3}$", ErrorMessage = "Currency pair must be in correct format")]
        private string CurrencyPair { get; }

        [Argument(1, Description = "Amount exchanged. Default value 1.")]
        private double Amount { get; } = 1;

        private void OnExecute(CommandLineApplication app)
        {
            if (string.IsNullOrEmpty(CurrencyPair))
            {
                app.ShowHelp();
            }

            var currencies = CurrencyPair.Split('/');
            var baseCurrencyISO = currencies[0];
            var quoteCurrencyISO = currencies[1];
            try
            {
                var exchanger = new RateExchanger(new HardCodedCurrencyPairRepository());
                var result = exchanger.Exchage(baseCurrencyISO, quoteCurrencyISO, (decimal)Amount);
                Console.WriteLine(result.Amount);
            }
            catch (RateNotFoundException)
            {
                Console.WriteLine("Rate not found.");
            }
            catch (Exception)
            {
                Console.WriteLine("Unknown error.");
            }
        }
    }
}
