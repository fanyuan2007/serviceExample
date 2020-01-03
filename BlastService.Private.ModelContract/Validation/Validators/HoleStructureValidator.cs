using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for HoleStructure requests.
    /// </summary>
    public class HoleStructureValidator : ValidatorBase<HoleStructure>
    {
        /// <summary>
        /// Instantiates a new HoleStructure validator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public HoleStructureValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single HoleStructure request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context object is not a HoleStructure request.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The HoleStructure request to validate.</param>
        /// <returns>True if there were no errors found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as HoleStructure;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (Double.IsNaN(ctx.Azimuth) || 
                ctx.Azimuth < 0.0 || 
                ctx.Azimuth > 360.0)
            {
                var message = String.Format(_fmtDoubleLimits, nameof(ctx.Azimuth), 0.0, 360.0);
                var error = new StringResult(nameof(ctx.Azimuth), message);
                currentNode.AddResult(error);
            }
            if (Double.IsNaN(ctx.Dip) || 
                ctx.Dip < -90.0 || 
                ctx.Dip > 90.0)
            {
                var message = String.Format(_fmtDoubleLimits, nameof(ctx.Dip), -90.0, 90.0);
                var error = new StringResult(nameof(ctx.Dip), message);
                currentNode.AddResult(error);
            }
            if (Double.IsNaN(ctx.Length) || 
                ctx.Length < 0)
            {
                var message = String.Format(_fmtDoubleGreaterThanEqualTo, nameof(ctx.Length), 0.0);
                var error = new StringResult(nameof(ctx.Length), message);
                currentNode.AddResult(error);
            }
            if (ctx.Collar == null)
            {
                var message = String.Format(_fmtNull, nameof(ctx.Collar));
                var error = new StringResult(nameof(ctx.Collar), message);
                currentNode.AddResult(error);
            }
            if (ctx.Toe == null)
            {
                var message = String.Format(_fmtNull, nameof(ctx.Toe));
                var error = new StringResult(nameof(ctx.Toe), message);
                currentNode.AddResult(error);
            }

            if (ctx.Collar != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.Collar));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.Collar, ref childNode);
            }
            if (ctx.Toe != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.Toe));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.Toe, ref childNode);
            }

            return currentNode.RootResult.ErrorCount == 0;
        }
    }
}
