using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for Pattern requests.
    /// </summary>
    public class PatternValidator : ValidatorBase<PatternRequest>
    {
        /// <summary>
        /// Instantiates a new Pattern validator with provided settings
        /// and the owning RequestValidator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">RequestValidator that owns this validator.</param>
        public PatternValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {
        }

        /// <summary>
        /// Validates a single PatternRequest object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context is not a PatternRequest.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The PatternRequest object to validate.</param>
        /// <returns>True if there were no errors found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as PatternRequest;
            if (ctx == null)
            {
                throw new ArgumentException(_contextCannotProcess);
            }

            var currentNode = result as NodeResult;
            if (currentNode == null)
            {
                throw new ArgumentException(_validatorBadErrorNode);
            }

            if (ctx.NameBasedProperties == null)
            {
                var message = String.Format(_fmtNull, nameof(ctx.NameBasedProperties));
                var error = new StringResult(nameof(ctx.NameBasedProperties), message);
                currentNode.AddResult(error);
            }

            if (String.IsNullOrWhiteSpace(ctx.Stage))
            {
                var message = String.Format(_fmtNullEmptyWhitespace, nameof(ctx.Stage));
                var error = new StringResult(nameof(ctx.Stage), message);
                currentNode.AddResult(error);
            }

            if (Double.IsNaN(ctx.FaceAngle) || 
                ctx.FaceAngle < -90 || 
                ctx.FaceAngle > 90)
            {
                var message = String.Format(_fmtDoubleLimits, nameof(ctx.FaceAngle), -90, 90);
                var error = new StringResult(nameof(ctx.FaceAngle), message);
                currentNode.AddResult(error);
            }

            if (Double.IsNaN(ctx.SubDrill))
            {
                var message = String.Format(_fmtDoubleValid, nameof(ctx.SubDrill));
                var error = new StringResult(nameof(ctx.SubDrill), message);
                currentNode.AddResult(error);
            }

            if (ctx.Bench != null && 
                String.IsNullOrWhiteSpace(ctx.Bench))
            {
                var message = String.Format(_fmtEmptyWhitespace, nameof(ctx.Bench));
                var error = new StringResult(nameof(ctx.Bench), message);
                currentNode.AddResult(error);
            }

            if (ctx.Pit != null && 
                String.IsNullOrWhiteSpace(ctx.Pit))
            {
                var message = String.Format(_fmtEmptyWhitespace, nameof(ctx.Pit));
                var error = new StringResult(nameof(ctx.Pit), message);
                currentNode.AddResult(error);
            }

            if (ctx.Phase != null && 
                String.IsNullOrWhiteSpace(ctx.Phase))
            {
                var message = String.Format(_fmtEmptyWhitespace, nameof(ctx.Phase));
                var error = new StringResult(nameof(ctx.Phase), message);
                currentNode.AddResult(error);
            }

            if (ctx.Area != null && 
                (Double.IsNaN(ctx.Area.GetValueOrDefault()) || 
                 ctx.Area.GetValueOrDefault() <= 0.0))
            {
                var message = String.Format(_fmtDoubleGreaterThan, nameof(ctx.Area), 0.0);
                var error = new StringResult(nameof(ctx.Area), message);
                currentNode.AddResult(error);
            }

            if (ctx.Volume != null && 
                (Double.IsNaN(ctx.Volume.GetValueOrDefault()) || 
                 ctx.Volume.GetValueOrDefault() <= 0.0))
            {
                var message = String.Format(_fmtDoubleGreaterThan, nameof(ctx.Volume), 0.0);
                var error = new StringResult(nameof(ctx.Volume), message);
                currentNode.AddResult(error);
            }

            if (String.IsNullOrWhiteSpace(ctx.HoleUsage))
            {
                var message = String.Format(_fmtNullEmptyWhitespace, nameof(ctx.HoleUsage));
                var error = new StringResult(nameof(ctx.HoleUsage), message);
                currentNode.AddResult(error);
            }

            if (ctx.GeologyCode != null && 
                String.IsNullOrWhiteSpace(ctx.GeologyCode))
            {
                var message = String.Format(_fmtNullEmptyWhitespace, nameof(ctx.GeologyCode));
                var error = new StringResult(nameof(ctx.GeologyCode), message);
                currentNode.AddResult(error);
            }

            if (String.IsNullOrWhiteSpace(ctx.Purpose))
            {
                var message = String.Format(_fmtNullEmptyWhitespace, nameof(ctx.Purpose));
                var error = new StringResult(nameof(ctx.Purpose), message);
                currentNode.AddResult(error);
            }

            if (ctx.PatternTemplateName != null && 
                string.IsNullOrWhiteSpace(ctx.PatternTemplateName))
            {
                var message = String.Format(_fmtEmptyWhitespace, nameof(ctx.PatternTemplateName));
                var error = new StringResult(nameof(ctx.PatternTemplateName), message);
                currentNode.AddResult(error);
            }

            if (ctx.ChargingTemplateName != null && 
                string.IsNullOrWhiteSpace(ctx.ChargingTemplateName))
            {
                var message = String.Format(_fmtNullEmptyWhitespace, nameof(ctx.ChargingTemplateName));
                var error = new StringResult(nameof(ctx.ChargingTemplateName), message);
                currentNode.AddResult(error);
            }

            if (ctx.MaxHoleFired != null && 
                ctx.MaxHoleFired < 1)
            {
                var message = String.Format(_fmtIntGreaterThanEqualTo, nameof(ctx.MaxHoleFired), 1);
                var error = new StringResult(nameof(ctx.MaxHoleFired), message);
                currentNode.AddResult(error);
            }

            if (ctx.MaxWeightFired != null && 
                (Double.IsNaN(ctx.MaxWeightFired.GetValueOrDefault()) || 
                 ctx.MaxWeightFired <= 0.0))
            {
                var message = String.Format(_fmtDoubleGreaterThan, nameof(ctx.MaxWeightFired), 0.0);
                var error = new StringResult(nameof(ctx.MaxWeightFired), message);
                currentNode.AddResult(error);
            }

            if (Double.IsNaN(ctx.PowderFactor) || 
                ctx.PowderFactor <= 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThan, nameof(ctx.PowderFactor), 0.0);
                var error = new StringResult(nameof(ctx.PowderFactor), message);
                currentNode.AddResult(error);
            }

            if (Double.IsNaN(ctx.RockFactor) || 
                ctx.RockFactor <= 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThan, nameof(ctx.RockFactor), 0.0);
                var error = new StringResult(nameof(ctx.RockFactor), message);
                currentNode.AddResult(error);
            }

            if (Double.IsNaN(ctx.RockSG) || 
                ctx.RockSG <= 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThan, nameof(ctx.RockSG), 0.0);
                var error = new StringResult(nameof(ctx.RockSG), message);
                currentNode.AddResult(error);
            }

            if (ctx.DesignBoundary == null)
            {
                var message = String.Format(_fmtNull, nameof(ctx.DesignBoundary));
                var error = new StringResult(nameof(ctx.DesignBoundary), message);
                currentNode.AddResult(error);
            }

            if (ctx.DesignBoundary != null &&
                ctx.DesignBoundary.Count() < 1)
            {
                var message = String.Format(_fmtCollectionItemCountAtLeast, nameof(ctx.DesignBoundary), 1);
                var error = new StringResult(nameof(ctx.DesignBoundary), message);
                currentNode.AddResult(error);
            }

            if (ctx.ActualBoundary != null && 
                ctx.ActualBoundary.Count() < 1)
            {
                var message = String.Format(_fmtCollectionItemCountAtLeast, nameof(ctx.ActualBoundary), 1);
                var error = new StringResult(nameof(ctx.ActualBoundary), message);
                currentNode.AddResult(error);
            }

            if (ctx.Holes != null && 
                ctx.Holes.Count() == 0)
            {
                var message = String.Format(_fmtCollectionItemCountAtLeast, nameof(ctx.Holes), 1);
                var error = new StringResult(nameof(ctx.Holes), message);
                currentNode.AddResult(error);
            }

            if (ctx.NameBasedProperties != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.NameBasedProperties));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.NameBasedProperties, ref childNode);
            }
            
            if (ctx.HoleLength != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.HoleLength));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.HoleLength, ref childNode);
            }

            if (ctx.Scoring != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.Scoring));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.Scoring, ref childNode);
            }

            if (ctx.DesignFragmentation != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.DesignFragmentation));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.DesignFragmentation, ref childNode);
            }

            if (ctx.ActualFragmentation != null)
            {
                IValidatorResult childNode = new NodeResult(nameof(ctx.ActualFragmentation));
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.ActualFragmentation, ref childNode);
            }

            if (ctx.DesignBoundary != null) 
            {
                int index = 0;
                var childListNode = new NodeResult(nameof(ctx.DesignBoundary));
                currentNode.AddResult(childListNode);

                foreach (var boundary in ctx.DesignBoundary)
                {
                    if (Settings.MaxErrors > 0 && 
                        result.RootResult.ErrorCount >= Settings.MaxErrors)
                    {
                        break;
                    }

                    IValidatorResult childItemNode = new NodeResult(index.ToString());
                    childListNode.AddResult(childItemNode);

                    Owner.Validate(boundary, ref childItemNode);
                    index++;
                }
            }

            if (ctx.ActualBoundary != null)
            {
                int index = 0;
                var childListNode = new NodeResult(nameof(ctx.ActualBoundary));
                currentNode.AddResult(childListNode);

                foreach (var boundary in ctx.ActualBoundary)
                {
                    if (Settings.MaxErrors > 0 &&
                        result.RootResult.ErrorCount >= Settings.MaxErrors)
                    {
                        break;
                    }

                    IValidatorResult childItemNode = new NodeResult(index.ToString());
                    childListNode.AddResult(childItemNode);

                    Owner.Validate(boundary, ref childItemNode);
                    index++;
                }
            }

            if (ctx.Holes != null)
            {
                int index = 0;
                var childListNode = new NodeResult(nameof(ctx.Holes));
                currentNode.AddResult(childListNode);

                foreach (var hole in ctx.Holes)
                {
                    if (Settings.MaxErrors > 0 &&
                        result.RootResult.ErrorCount >= Settings.MaxErrors)
                    {
                        break;
                    }

                    IValidatorResult childItemNode = new NodeResult(index.ToString());
                    childListNode.AddResult(childItemNode);

                    Owner.Validate(hole, ref childItemNode);
                    index++;
                }
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
