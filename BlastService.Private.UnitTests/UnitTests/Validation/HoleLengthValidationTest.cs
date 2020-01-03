using NUnit.Framework;

using BlastService.Private.ModelContract.Validation;
using BlastService.Private.ModelContract;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class HoleLengthValidationTest
    {
        private HoleLength _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new HoleLength()
            {
                Average = 5.0,
                Total = 18.5,
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
        [TestCase(-1.0, false)]
        [TestCase(0.0, true)]
        [TestCase(1.0, true)]
        public void AverageTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Average = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(-1.0, false)]
        [TestCase(0.0, true)]
        [TestCase(1.0, true)]
        public void TotalTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Total = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
    }
}
