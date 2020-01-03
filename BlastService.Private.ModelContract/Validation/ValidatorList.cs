using BlastService.Private.ModelContract.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation
{
    public interface IValidatorList
    {
        bool Validate(object obj, ref IValidatorResult result);
        IValidationSettings Settings { get; set; }
    }

    public class ValidatorList : List<IValidator>, IValidatorList
    {
        /// <summary>
        /// Constructs a new ValidatorList.
        /// </summary>
        /// <param name="settings"></param>
        public ValidatorList(IValidationSettings settings)
            : base()
        {
            Settings = settings;
        }

        /// <summary>
        /// Constructs a new ValidatorList with a set of specified validators.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="validators"></param>
        public ValidatorList(IValidationSettings settings,
                             IEnumerable<IValidator> validators)
            : base()
        {
            Settings = settings;
            this.AddRange(validators);
        }

        

        /// <summary>
        /// The validation settings to be used during validation.
        /// </summary>
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
                foreach (var validation in this)
                {
                    validation.Settings = value;
                }
            }
        }
        private IValidationSettings _settings;

        /// <summary>
        /// Validates a single object using the validators stored in this list.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown if result is an end result node (cannot have children) 
        /// or if the object cannot be validated.</exception>
        /// <param name="obj">The object to validate.</param>
        /// <param name="result">Current validation result node.</param>
        /// <returns>True if no errors were found. False otherwise.</returns>
        public bool Validate(object obj, ref IValidatorResult result)
        {
            if (!(result is NodeResult))
            {
                throw new ArgumentException("Parent result is already an end result.");
            }

            var objValidator = this.First(validator => validator.CanValidate(obj));
            if (objValidator != null)
            {
                objValidator.Validate(ref result, obj);
            }
            else
            {
                throw new ArgumentException(String.Format("Cannot validate object of type: {0}", obj.GetType()));
            }

            return result.ErrorCount == 0;
        }
    }
}
