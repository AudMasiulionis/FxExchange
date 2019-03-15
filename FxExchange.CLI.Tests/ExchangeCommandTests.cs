using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FxExchange.CLI.Tests
{
    [TestFixture]
    public class ExchangeCommandTests
    {
        public static IEnumerable<TestCaseData> TestData()
        {
            yield return new TestCaseData("EUR/USD 1", new ExchangeCommand("EUR", "USD", 1m));
            yield return new TestCaseData("EUR/USD 1.1", new ExchangeCommand("EUR", "USD", 1.1m));
            yield return new TestCaseData("EUR/USD 1.2", new ExchangeCommand("EUR", "USD", 1.2m));
        }

        [Test]
        [TestCaseSource(nameof(TestData))]
        public void ExchangeCommand_ShouldBenItializedCorrectly(string command, ExchangeCommand expected)
        {
            var result = new ExchangeCommand(command);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("EUR")]
        [TestCase("EUR/sss")]
        [TestCase("EUR/USD")]
        [TestCase("EUR/USD .")]
        [TestCase("EUR/USD X")]
        public void ExchangeCommand_ShouldThrowFormatException_WithInvalidCommand(string command)
        {
            Action act = () => new ExchangeCommand(command);
            Assert.Throws<FormatException>(new TestDelegate(act));
        }
    }
}
