using System;

using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class ChargeIntervalValidationTest
    {
        private ChargeInterval _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new ChargeInterval()
            {
                Amount = 100.00,
                Consumable = "ANFO",
                Deck = "Needs Scrubbing",
                From = 2,
                To = 4,
            };
            
            _validator = RequestValidatorFactory.CreateValidator();
        }

        [TearDown]
        public void TearDown()
        {
            _request = null;
            _validator = null;
        }

        // Double comparison
        [TestCase(0.0, true)]
        [TestCase(-1.0, false)]
        [TestCase(Double.NaN, false)]
        public void FromTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.From = value;
            _request.From = Math.Min(_request.From, _request.To); // Testing ranges will be done elsewhere.

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(0.0, true)]
        [TestCase(-1.0, false)]
        [TestCase(Double.NaN, false)]
        public void ToTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.To = value;
            _request.From = Math.Min(_request.From, _request.To); // Testing ranges will be done elsewhere.

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void FromGreaterThanToTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.To = 2;
            _request.From = 3;

            var result = _validator.Validate(_request, out errors);
            Assert.IsFalse(result);
        }

        // Double comparison
        [Test]
        public void FromLessThanEqualToToTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // From <= To
            _request.To = 5.0;
            _request.From = 10.0;
            Assert.IsFalse(_validator.Validate(_request, out errors));
            _request.From = 5.0;
            Assert.IsTrue(_validator.Validate(_request, out errors));
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void ConsumableTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Consumable = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(0.0, true)]
        [TestCase(-1.0, false)]
        [TestCase(Double.NaN, false)]
        public void AmountTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Amount = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" \t\r\n", false)]
        public void DeckTest(string value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Deck = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
    }
}
