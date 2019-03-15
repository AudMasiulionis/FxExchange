using FxExchange.Domain;
using FxExchange.Domain.Exceptions;
using FxExchange.Infrastructure;
using System;
using System.Text.RegularExpressions;

namespace FxExchange.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var exchanger = new RateExchanger(new HardCodedCurrencyPairRepository());
                Console.WriteLine("Usage: Exchange <currency pair> <amount to exchnge>");
                var command = new ExchangeCommand(Console.ReadLine());
                var result = exchanger.Exchage(command.BaseCurrencyISO, command.QuoteCurrencyISO, command.Amount);
                Console.WriteLine(result.Amount);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid command format provided.");
            }
            catch (RateNotFoundException)
            {
                Console.WriteLine("Provided rate not found.");
            }
            catch (Exception)
            {
                Console.WriteLine("Unknown error.");
            }
        }
    }
}
