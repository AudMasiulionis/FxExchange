using FxExchange.Domain;
using FxExchange.Domain.Exceptions;
using FxExchange.Domain.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Fx.Exchange.Domain.Tests
{
    [TestFixture]
    public class RateExchangerTests
    {
        private MockRepository mockRepository;
        private Mock<ICurrencyPairRepository> mockCurrencyPairRepository;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockCurrencyPairRepository?.Reset();
            this.mockCurrencyPairRepository = mockRepository.Create<ICurrencyPairRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            this.mockRepository.VerifyAll();
        }

        private RateExchanger CreateRateExchanger()
        {
            return new RateExchanger(this.mockCurrencyPairRepository.Object);
        }

        public static IEnumerable<TestCaseData> Exchage_WithFoundBaseCurrency_ShouldReturnCorrectResult_TestData()
        {
            yield return new TestCaseData("EUR", "DKK", 1.0m, new Money(7.4394m, "EUR"));
            yield return new TestCaseData("EUR", "DKK", 2.0m, new Money(14.8788m, "EUR"));
        }

        [Test]
        [TestCaseSource(nameof(Exchage_WithFoundBaseCurrency_ShouldReturnCorrectResult_TestData))]
        public void Exchage_WithFoundBaseCurrency_ShouldReturnCorrectResult(string iso1, string iso2, decimal amount, Money expected)
        {
            this.mockCurrencyPairRepository.Setup(x => x.GetByBaseCurrency(It.IsAny<string>()))
                .Returns(new CurrencyPair
                {
                    BaseCurrency = "EUR",
                    BaseAmount = 100m,
                    QuoteCurrency = "DKK",
                    QuoteAmount = 743.94m
                });

            var unitUnderTest = this.CreateRateExchanger();

            var result = unitUnderTest.Exchage(
                iso1,
                iso2,
                amount);

            Assert.AreEqual(expected, result);
        }

        public static IEnumerable<TestCaseData> Exchage_WithFoundQuoteCurrency_ShouldReturnCorrectResult_TestData()
        {
            yield return new TestCaseData("DKK", "EUR", 1.0m, new Money(0.1344194424281528080221523241m, "DKK"));
        }

        [Test]
        [TestCaseSource(nameof(Exchage_WithFoundQuoteCurrency_ShouldReturnCorrectResult_TestData))]
        public void Exchage_WithFoundQuoteCurrency_ShouldReturnCorrectResult(string iso1, string iso2, decimal amount, Money expected)
        {
            this.mockCurrencyPairRepository.Setup(x => x.GetByBaseCurrency(It.IsAny<string>())).Returns<Money>(null);
            this.mockCurrencyPairRepository.Setup(x => x.GetByQuoteCurrency(It.IsAny<string>()))
                .Returns(new CurrencyPair
                {
                    BaseCurrency = "EUR",
                    BaseAmount = 100m,
                    QuoteCurrency = "DKK",
                    QuoteAmount = 743.94m
                });

            var unitUnderTest = this.CreateRateExchanger();

            var result = unitUnderTest.Exchage(
                iso1,
                iso2,
                amount);

            Assert.AreEqual(expected, result);
        }

        public static IEnumerable<TestCaseData> Exchage_WithNotMatchingBaseCurrencies_ShouldReturnCorrectResult_TestData()
        {
            yield return new TestCaseData("EUR", "USD", 1.0m, new Money(1.1218953114867819818732940236m, "EUR"));
        }

        [Test]
        [TestCaseSource(nameof(Exchage_WithNotMatchingBaseCurrencies_ShouldReturnCorrectResult_TestData))]
        public void Exchage_WithNotMatchingBaseCurrencies_ShouldReturnCorrectResult(string iso1, string iso2, decimal amount, Money expected)
        {
            this.mockCurrencyPairRepository.Setup(x => x.GetByBaseCurrency("EUR"))
                .Returns(new CurrencyPair
                {
                    BaseCurrency = "EUR",
                    BaseAmount = 100m,
                    QuoteCurrency = "DKK",
                    QuoteAmount = 743.94m
                });

            this.mockCurrencyPairRepository.Setup(x => x.GetByBaseCurrency("USD"))
                .Returns(new CurrencyPair
                {
                    BaseCurrency = "USD",
                    BaseAmount = 100m,
                    QuoteCurrency = "DKK",
                    QuoteAmount = 663.11m
                });

            var unitUnderTest = this.CreateRateExchanger();

            var result = unitUnderTest.Exchage(
                iso1,
                iso2,
                amount);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Exchage_WithNotFoundCurrencies_ShouldThrowRateNotFoundException()
        {
            this.mockCurrencyPairRepository.Setup(x => x.GetByBaseCurrency(It.IsAny<string>())).Returns<Money>(null);
            this.mockCurrencyPairRepository.Setup(x => x.GetByQuoteCurrency(It.IsAny<string>())).Returns<Money>(null);

            var unitUnderTest = this.CreateRateExchanger();

            Action act = () => unitUnderTest.Exchage(
                "XXX",
                "DKK",
                1);

            Assert.Throws<RateNotFoundException>(new TestDelegate(act));
        }

        [Test]
        public void Exchage_WithZeroAmount_ShouldReturnZero()
        {
            var unitUnderTest = this.CreateRateExchanger();

            var result = unitUnderTest.Exchage(
                "",
                "",
                0);

            Assert.AreEqual(new Money(0, ""), result);
        }
    }
}
