using BlastService.Private.ModelContract.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation
{
    /// <summary>
    /// Factory for creating RequestValidator objects to specified parameters.
    /// </summary>
    public static class RequestValidatorFactory
    {
        private static Dictionary<Type, Func<IValidationSettings, RequestValidator, IValidator>> _validatorConstructorDict;
        private static bool _isInitialised;

        /// <summary>
        /// Creates a RequestValidator with default validators.
        /// </summary>
        /// <param name="maxErrors">Maximum number of errors to halt on. Defaults to 10.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <returns>A RequestValidator with default validators.</returns>
        public static RequestValidator CreateValidator(int maxErrors = 10)
        {
            if (maxErrors < 0)
            {
                throw new ArgumentOutOfRangeException("maxErrors");
            }
            
            if (!_isInitialised)
            {
                Initialise();
            }

            IValidationSettings settings = new ValidationSettings() { MaxErrors = maxErrors };
            var requestValidator = new RequestValidator(settings);

            var validators = GetDefaultValidators(requestValidator, settings);
            foreach (IValidator validator in validators)
            {
                requestValidator.AddValidator(validator);
            }

            return requestValidator;
        }

        /// <summary>
        /// Creates a new RequestValidator targeting a specific set of request types.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <param name="targetTypes">The set of request types the validator can handle.</param>
        /// <param name="maxErrors">The maximum number of errors to try and log.</param>
        /// <returns>A request validator.</returns>
        public static RequestValidator CreateValidator(IEnumerable<Type> targetTypes, int maxErrors = 10)
        {
            if (maxErrors < 0)
            {
                throw new ArgumentOutOfRangeException("maxErrors");
            }

            if (!_isInitialised)
            {
                Initialise();
            }

            IValidationSettings settings = new ValidationSettings() { MaxErrors = maxErrors };
            var requestValidator = new RequestValidator(settings);

            var validators = targetTypes.Select(type => GetValidator(type, settings, requestValidator));
            foreach(IValidator validator in validators)
            {
                requestValidator.AddValidator(validator);
            }

            return requestValidator;
        }

        private static void Initialise()
        {
            _isInitialised = true;

            _validatorConstructorDict = new Dictionary<Type, Func<IValidationSettings, RequestValidator, IValidator>>();

            _validatorConstructorDict.Add(typeof(ActualFragmentation), (s, o) => new ActualFragmentationValidator(s,o));
            _validatorConstructorDict.Add(typeof(BaseProperty), (s, o) => new BasePropertyValidator(s, o));
            _validatorConstructorDict.Add(typeof(ChargeInfo), (s, o) => new ChargeInfoValidator(s, o));
            _validatorConstructorDict.Add(typeof(ChargeInterval), (s, o) => new ChargeIntervalValidator(s, o));
            _validatorConstructorDict.Add(typeof(DesignFragmentation), (s, o) => new DesignFragmentationValidator(s, o));
            _validatorConstructorDict.Add(typeof(GPSCoordinate), (s, o) => new GPSCoordinateValidator(s, o));
            _validatorConstructorDict.Add(typeof(HoleLength), (s, o) => new HoleLengthValidator(s, o));
            _validatorConstructorDict.Add(typeof(HoleStructure), (s, o) => new HoleStructureValidator(s, o));
            _validatorConstructorDict.Add(typeof(HoleRequest), (s, o) => new HoleValidator(s, o));
            _validatorConstructorDict.Add(typeof(LocalTransformation), (s, o) => new LocalTransformationValidator(s, o));
            _validatorConstructorDict.Add(typeof(Transformation), (s, o) => new TransformationValidator(s, o));
            _validatorConstructorDict.Add(typeof(MetricScore), (s, o) => new MetricScoreValidator(s, o));
            _validatorConstructorDict.Add(typeof(NamedBaseProperty), (s, o) => new NamedBasePropertyValidator(s, o));
            _validatorConstructorDict.Add(typeof(PatternRequest), (s, o) => new PatternValidator(s, o));
            _validatorConstructorDict.Add(typeof(PolyGeom), (s, o) => new PolyGeomValidator(s, o));
            _validatorConstructorDict.Add(typeof(ProjectRequest), (s, o) => new ProjectValidator(s, o));
            _validatorConstructorDict.Add(typeof(Scoring), (s, o) => new ScoringValidator(s, o));
            _validatorConstructorDict.Add(typeof(UTCTimeZone), (s, o) => new UTCTimeZoneValidator(s, o));
        }

        private static IValidator GetValidator(Type targetType, IValidationSettings settings, RequestValidator owner)
        {
            if (_validatorConstructorDict.ContainsKey(targetType))
            {
                return _validatorConstructorDict[targetType].Invoke(settings, owner);
            }
            else
            {
                throw new NotSupportedException("Target type does not have a validator associated.");
            }
        }

        /// <summary>
        /// Instantiates default validators for the validator list.
        /// <param name="owner">The owner of the validators.</param>
        /// </summary>
        private static List<IValidator> GetDefaultValidators(RequestValidator owner, IValidationSettings settings)
        {
            var types = new List<Type>()
            {
                typeof(ActualFragmentation),
                typeof(BaseProperty),
                typeof(ChargeInfo),
                typeof(ChargeInterval),
                typeof(DesignFragmentation),
                typeof(GPSCoordinate),
                typeof(HoleLength),
                typeof(HoleStructure),
                typeof(HoleRequest),
                typeof(LocalTransformation),
                typeof(MetricScore),
                typeof(NamedBaseProperty),
                typeof(PatternRequest),
                typeof(PolyGeom),
                typeof(ProjectRequest),
                typeof(Scoring),
                typeof(Transformation),
                typeof(UTCTimeZone),
            };

            return types.Select(type => GetValidator(type, settings, owner)).ToList();
        }
    }
}
