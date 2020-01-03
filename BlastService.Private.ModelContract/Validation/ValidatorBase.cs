using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BlastService.Private.ModelContract.Validation
{
    public abstract class ValidatorBase<T> : IValidator where T : class
    {
        protected ValidatorBase(IValidationSettings settings, RequestValidator owner)
        {
            Settings = settings;
            Owner = owner;
        }

        public RequestValidator Owner
        {
            get; private set;
        }

        public IValidationSettings Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Settings");
                }
                _settings = value;
            }
        }
        private IValidationSettings _settings;
        #region Error Messages and Formats
        protected static string _contextCannotProcess = "Context cannot be processed by this validator.";
        protected static string _validatorBadErrorNode = "Attempting to attach results to a childless node.";
        
        protected static string _fmtNull = "{0} must not be null.";
        protected static string _fmtNullEmptyWhitespace = "{0} must not be null, empty, or only whitespaces.";
        protected static string _fmtEmptyWhitespace = "{0} must not be empty or only whitespaces.";

        protected static string _fmtDoubleValid = "{0} must be a valid real number.";
        protected static string _fmtDoubleLimits = "{0} must be a valid real number between {1} and {2}.";
        protected static string _fmtDoubleGreaterThan = "{0} must be a valid real number greater than {1}.";
        protected static string _fmtDoubleGreaterThanEqualTo = "{0} must be a valid real number greater than or equal to {1}.";
        protected static string _fmtDoubleLessThan = "{0} must be a valid real number less than {1}";

        protected static string _fmtIntGreaterThanEqualTo = "{0} must be an integer greater than or equal to {1}";
        protected static string _fmtIntLimits = "{0} must be an integer between {1} and {2}";

        protected static string _fmtCollectionItemCountAtLeast = "{0} must have at least {1} element(s).";

        #endregion

        /// <summary>
        /// Gets whether a given object can be validated.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool CanValidate(object obj)
        {
            return obj.GetType() == typeof(T);
        }
        public abstract bool Validate(ref IValidatorResult result, object context);
    }
}
