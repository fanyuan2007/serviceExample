using System;
using System.Collections.Generic;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for GPSCoordinate requests.
    /// </summary>
    public class GPSCoordinateValidator : ValidatorBase<GPSCoordinate>
    {
        /// <summary>
        /// Instantiates a new GPSCoordinate validator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public GPSCoordinateValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single GPSCoordinate request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context object is not a GPSCoordinate request.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The GPSCoordinate request to validate.</param>
        /// <returns>True if no errors were found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as GPSCoordinate;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (Double.IsNaN(ctx.Latitude) || 
                ctx.Latitude <= -90.0 || 
                ctx.Latitude >= 90.0)
            {
                var message = String.Format(_fmtDoubleLimits, nameof(ctx.Latitude), -90.0, 90.0);
                var error = new StringResult(nameof(ctx.Latitude), message);
                currentNode.AddResult(error);
            }
            if (Double.IsNaN(ctx.Longitude) || 
                ctx.Longitude <= -180.0 || 
                ctx.Longitude >= 180.0)
            {
                var message = String.Format(_fmtDoubleLimits, nameof(ctx.Longitude), -180.0, 180.0);
                var error = new StringResult(nameof(ctx.Longitude), message);
                currentNode.AddResult(error);
            }
            if (Double.IsNaN(ctx.Elevation))
            {
                var message = String.Format(_fmtDoubleValid, nameof(ctx.Elevation));
                var error = new StringResult(nameof(ctx.Elevation), message);
                currentNode.AddResult(error);
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
