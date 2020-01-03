using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for UTCTimeZone requests.
    /// </summary>
    public class UTCTimeZoneValidator : ValidatorBase<UTCTimeZone>
    {
        /// <summary>
        /// Instantiates a new UTCTimeZone validator with provided settings
        /// and the owning RequestValidator
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public UTCTimeZoneValidator(IValidationSettings settings, RequestValidator owner)
            : base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single UTCTimeZone request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context is not a UTCTimeZone object.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The UTCTimeZone request to validate.</param>
        /// <returns>True if there were no errors found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as UTCTimeZone;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new NotImplementedException(_validatorBadErrorNode);
            }

            // UTC timezone hour offsets
            // [-12, 14]
            if (ctx.OffsetHours <= -13 || ctx.OffsetHours >= 15)
            {
                var message = String.Format(_fmtIntLimits, nameof(ctx.OffsetHours), -12, 14);
                var error = new StringResult(nameof(ctx.OffsetHours), message);
                currentNode.AddResult(error);
            }

            // TimeSpan.OffsetMinutes limits as specified by .NET framework.
            // [-59, 59]
            if (ctx.OffsetMinutes <= -60 || ctx.OffsetMinutes >= 60)
            {
                var message = String.Format(_fmtIntLimits, nameof(ctx.OffsetMinutes), -60, 60);
                var error = new StringResult(nameof(ctx.OffsetMinutes), message);
                currentNode.AddResult(error);
            }

            if (String.IsNullOrWhiteSpace(ctx.IdName))
            {
                var message = String.Format(_fmtNullEmptyWhitespace, nameof(ctx.IdName));
                var error = new StringResult(nameof(ctx.IdName), message);
                currentNode.AddResult(error);
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
