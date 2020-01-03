using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    public class NamedBasePropertyValidator : ValidatorBase<NamedBaseProperty>
    {
        public NamedBasePropertyValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {

        }

        /// <summary>
        /// Validates a single NamedBaseProperty request.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context is not a NamedBaseProperty</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The NamedBaseProperty request to validate.</param>
        /// <returns>True if no errors were found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as NamedBaseProperty;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (ctx.BaseProperties == null)
            {
                var message = String.Format(_fmtNull, nameof(ctx.BaseProperties));
                var error = new StringResult(nameof(ctx.BaseProperties), message);
                currentNode.AddResult(error);
            }

            if (String.IsNullOrWhiteSpace(ctx.Name))
            {
                var message = String.Format(_fmtNullEmptyWhitespace, nameof(ctx.Name));
                var error = new StringResult(nameof(ctx.Name), message);
                currentNode.AddResult(error);
            }

            if (ctx.BaseProperties != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.BaseProperties));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.BaseProperties, ref childNode);
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
