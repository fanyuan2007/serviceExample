
using NUnit.Framework;

using BlastService.Private.ModelContract;
using BlastService.Private.ModelContract.Validation;

namespace BlastService.Private.Tests.UnitTests.Validation
{
    [TestFixture]
    public class LocalTransformationValidationTest
    {
        private LocalTransformation _request;
        private RequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _request = new LocalTransformation()
            {
                Translation = new Transformation()
                {
                    East = 10,
                    Elev = 10,
                    North = 10,
                },
                Rotation = new Transformation()
                {
                    East = 15,
                    Elev = 15,
                    North = 15,
                },
                Scaling = new Transformation()
                {
                    East = 25,
                    Elev = 25,
                    North = 25,
                },
            };

            _validator = RequestValidatorFactory.CreateValidator();
        }

        [TearDown]
        public void TearDown()
        {
            _request = null;
            _validator = null;
        }

        [Test]
        public void TranslationTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // NOT null
            _request.Translation = null; 
            Assert.IsFalse(_validator.Validate(_request, out errors));
        }

        [Test]
        public void RotationTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // NOT null
            _request.Rotation = null; 
            Assert.IsFalse(_validator.Validate(_request, out errors));
        }

        [Test]
        public void ScalingTest()
        {
            Assert.IsTrue(_validator.Validate(_request, out var errors));

            // NOT null
            _request.Scaling = null; 
            Assert.IsFalse(_validator.Validate(_request, out errors));
        }
    }
}
