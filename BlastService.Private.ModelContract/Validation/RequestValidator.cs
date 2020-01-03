using BlastService.Private.ModelContract.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation
{
    /// <summary>
    /// A validator class that evaluates the validity of model contract request classes.
    /// Can be set to break out early with a maximum limit on error count.
    /// </summary>
    public class RequestValidator
    {
        #region Lifecycle
        /// <summary>
        /// Instantiates a new validator with a set of specific validators.
        /// </summary>
        /// <param name="settings">The validation settings to use.</param>
        public RequestValidator(IValidationSettings settings)
        {
            _validators = new ValidatorList(settings);
        }
        #endregion

        #region Fields and Properties
        private ValidatorList _validators;
        #endregion

        #region Functions
        /// <summary>
        /// Adds a validator to this RequestValidator.
        /// </summary>
        /// <param name="validator"></param>
        public void AddValidator(IValidator validator)
        {
            _validators.Add(validator);
        }

        /// <summary>
        /// Validates a single object, presumably a request.
        /// Returns a list of errors discovered during validation.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        /// <param name="result">The validation result.</param>
        /// <param name="resultName">The name to use for the result. 
        /// Uses the typename of the request if null or empty.</param>
        /// <returns>True if no errors were found. False otherwise.</returns>
        public bool Validate(object request, out IValidatorResult result, string resultName = null)
        {
            resultName = String.IsNullOrWhiteSpace(resultName)
                ? request.GetType().Name
                : resultName;
            result = new NodeResult(resultName); 
            return _validators.Validate(request, ref result);
        }

        /// <summary>
        /// Validates a single object, presumably a request.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        /// <param name="result">The validation result.</param>
        /// <returns>True if no errors were found. False otherwise.</returns>
        public bool Validate(object request, ref IValidatorResult result)
        {
            return _validators.Validate(request, ref result);
        }
        #endregion
    }
}
