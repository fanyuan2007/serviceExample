using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

using NUnit.Framework;

using System;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class DesignFragmentationValidationTest
    {
        private DesignFragmentation _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            // Create a request that is (presumably) 100% valid, including optional properties.
            _request = new DesignFragmentation()
            {
                P10 = 1.0,
                P20 = 2.0,
                P30 = 3.0,
                P40 = 4.0,
                P50 = 5.0,
                P60 = 6.0,
                P70 = 7.0,
                P80 = 8.0,
                P90 = 9.0,
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
        [TestCase(null, true)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(Double.NaN, false)]
        public void P10Test(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.P10 = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(null, true)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(Double.NaN, false)]
        public void P20Test(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.P20 = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(null, true)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(Double.NaN, false)]
        public void P30Test(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.P30 = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(null, true)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(Double.NaN, false)]
        public void P40Test(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.P40 = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(null, true)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(Double.NaN, false)]
        public void P50Test(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.P50 = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(null, true)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(Double.NaN, false)]
        public void P60Test(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.P60 = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(null, true)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(Double.NaN, false)]
        public void P70Test(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.P70 = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(null, true)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(Double.NaN, false)]
        public void P80Test(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.P80 = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        // Double comparison
        [TestCase(null, true)]
        [TestCase(-1.0, false)]
        [TestCase(0.0, false)]
        [TestCase(Double.NaN, false)]
        public void P90Test(double? value, bool expected)
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));
            _request.P90 = value;

            var result = _validator.Validate(_request, out errors);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void AtLeastOneTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // At least one of Pxx must be set.
            _request.P10 = null;
            _request.P20 = null;
            _request.P30 = null;
            _request.P40 = null;
            _request.P50 = null;
            _request.P60 = null;
            _request.P70 = null;
            _request.P80 = null;
            _request.P90 = null;

            Assert.IsFalse(_validator.Validate(_request, out errors));
        }

        [Test]
        public void PValueChainTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // Pxx must not be smaller than a previous, lower-number Pxx.
            _request.P10 = 99;
            _request.P20 = 5;

            Assert.IsFalse(_validator.Validate(_request, out errors));
        }
    }
}
