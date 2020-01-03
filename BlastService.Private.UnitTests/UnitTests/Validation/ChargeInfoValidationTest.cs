using System;

using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class ChargeInfoValidationTest
    {
        private ChargeInfo _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new ChargeInfo()
            {
                Thickness = 50.0,
                Weight = 25.0,
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
        public void WeightTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Weight = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
        
        // Double comparison
        [TestCase(0.0, true)]
        [TestCase(-1.0, false)]
        [TestCase(Double.NaN, false)]
        public void ThicknessTest(double value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.Thickness = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }
    }
}
