using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for PolyGeom requests.
    /// </summary>
    public class PolyGeomValidator : ValidatorBase<PolyGeom>
    {
        /// <summary>
        /// Instantiates a new PolyGeom validator with provided settings
        /// and the owning RequestValidator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public PolyGeomValidator(IValidationSettings settings, RequestValidator owner)
            :base (settings, owner)
        {
        }

        /// <summary>
        /// Validates a single PolyGeom object.
        /// </summary>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The PolyGeom request to validate.</param>
        /// <returns>True if there were no errors found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as PolyGeom;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (ctx.PolyGeometry == null)
            {
                var message = String.Format(_fmtNull, nameof(ctx.PolyGeometry));
                var error = new StringResult(nameof(ctx.PolyGeometry), message);
                currentNode.AddResult(error);
            }

            if (ctx.PolyGeometry != null && 
                ctx.PolyGeometry.Count() < 2)
            {
                var message = String.Format(_fmtCollectionItemCountAtLeast, nameof(ctx.PolyGeometry), 2);
                var error = new StringResult(nameof(ctx.PolyGeometry), message);
                currentNode.AddResult(error);
            }

            if (ctx.PolyGeometry != null)
            {
                int index = 0;
                var childListNode = new NodeResult(nameof(ctx.PolyGeometry));
                currentNode.AddResult(childListNode);

                foreach (var poly in ctx.PolyGeometry)
                {
                    if (Settings.MaxErrors > 0 &&
                        result.RootResult.ErrorCount >= Settings.MaxErrors)
                    {
                        break;
                    }

                    IValidatorResult childItemNode = new NodeResult(index.ToString());
                    childListNode.AddResult(childItemNode);

                    Owner.Validate(poly, ref childItemNode);

                    index++;
                }
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
