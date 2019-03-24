using FxExchange.Domain;
using FxExchange.Domain.Exceptions;
using FxExchange.Domain.Models;
using Moq;
using NUnit.Framework;
using System;

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
            this.mockCurrencyPairRepository = mockRepository.Create<ICurrencyPairRepository>();
        }

        private Money TestData(string iso)
        {
            switch (iso)
            {
                case "XXX":
                    return new Money(10, iso);
                case "YYY":
                    return new Money(5, iso);
                default:
                    return null;
            }
        }

        [TearDown]
        public void TearDown()
        {
            this.mockRepository.VerifyAll();
        }

        private RateExchanger CreateRateExchanger()
        {
            this.mockCurrencyPairRepository.Setup(x => x.GetByBaseCurrency(It.IsAny<string>()))
                .Returns((string iso) => TestData(iso));
            return new RateExchanger(this.mockCurrencyPairRepository.Object);
        }

        [Test]
        [TestCase("XXX", "YYY", 1.0, 2)]
        [TestCase("XXX", "YYY", 2.0, 4)]
        public void Exchage_WithFoundBaseCurrency_ShouldReturnCorrectResult(string baseCurrencyISO, string quoteCurrencyISO, decimal amount, decimal expected)
        {
            var unitUnderTest = this.CreateRateExchanger();
            var result = unitUnderTest.Exchage(
                baseCurrencyISO,
                quoteCurrencyISO,
                amount);

            Assert.AreEqual(expected, result.Amount);
        }

        [Test]
        [TestCase("XXX", "111")]
        [TestCase("111", "XXX")]
        public void Exchage_WithNotFoundCurrencies_ShouldThrowRateNotFoundException(string baseCurrencyISO, string quoteCurrencyISO)
        {
            var unitUnderTest = this.CreateRateExchanger();
            Action act = () => unitUnderTest.Exchage(
                baseCurrencyISO,
                quoteCurrencyISO,
                1);

            Assert.Throws<RateNotFoundException>(new TestDelegate(act));
        }

        [Test]
        [TestCase("", "YYY", 1)]
        [TestCase("YYY", "", 1)]
        [TestCase("YYY", "YYY", 0)]
        [TestCase("YYY", "YYY", -1)]
        public void Exchage_WithIncorrectParameters_ShouldThrowArgumentException(string baseCurrencyISO, string quoteCurrencyISO, decimal amount)
        {
            var unitUnderTest = this.CreateRateExchanger();
            this.mockCurrencyPairRepository.Reset();

            Action act = () => unitUnderTest.Exchage(
                baseCurrencyISO,
                quoteCurrencyISO,
                amount);

            Assert.Throws<ArgumentException>(new TestDelegate(act));
        }

    }
}
