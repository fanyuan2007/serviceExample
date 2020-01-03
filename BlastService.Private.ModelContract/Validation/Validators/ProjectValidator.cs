using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for Project requests.
    /// </summary>
    public class ProjectValidator : ValidatorBase<ProjectRequest>
    {
        /// <summary>
        /// Instantiates a new Project validator with provided settings
        /// and the owning RequestValidator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public ProjectValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {
        }
        /// <summary>
        /// Validates a single Project request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context is not a ProjectRequest.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The project request to validate.</param>
        /// <returns>True if no errors were found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 && 
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as ProjectRequest;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (ctx.NameBasedProperties != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.NameBasedProperties));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.NameBasedProperties, ref childNode);
            } 
            else
            {
                var message = String.Format(_fmtNull, nameof(ctx.NameBasedProperties));
                var error = new StringResult(nameof(ctx.NameBasedProperties), message);
                currentNode.AddResult(error);
            }

            if (ctx.Description == null) 
            { 
                var message = String.Format(_fmtNull, nameof(ctx.Description));
                var error = new StringResult(nameof(ctx.Description), message);
                currentNode.AddResult(error);
            }
            
            if (ctx.TimeZone != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.TimeZone));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.TimeZone, ref childNode);
            }
            else 
            {
                var message = String.Format(_fmtNull, nameof(ctx.TimeZone));
                var error = new StringResult(nameof(ctx.TimeZone), message);
                currentNode.AddResult(error);
            }

            if (ctx.LocalTransformation != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.LocalTransformation));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.LocalTransformation, ref childNode);
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
