using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastService.Private.ModelContract.Validation.Validators
{
    /// <summary>
    /// A validator for Hole requests.
    /// </summary>
    public class HoleValidator : ValidatorBase<HoleRequest>
    {
        /// <summary>
        /// Instantiates a new HoleValidator.
        /// </summary>
        /// <param name="settings">Validation settings.</param>
        /// <param name="owner">The RequestValidator that owns this HoleValidator.</param>
        public HoleValidator(IValidationSettings settings, RequestValidator owner)
            :base(settings, owner)
        {
        }
    
        /// <summary>
        /// Validates a single HoleRequest request object.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the provided context object is not a HoleRequest.</exception>
        /// <param name="result">The validation results.</param>
        /// <param name="context">The HoleRequest request to validate.</param>
        /// <returns>True if no errors were found thus far. False otherwise.</returns>
        public override bool Validate(ref IValidatorResult result, object context)
        {
            if (Settings.MaxErrors > 0 &&
                result.RootResult.ErrorCount >= Settings.MaxErrors)
            {
                return false;
            }

            var ctx = context as HoleRequest;
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

            if (Double.IsNaN(ctx.AreaOfInfluence) || 
                ctx.AreaOfInfluence <= 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThan, nameof(ctx.AreaOfInfluence), 0.0);
                var error = new StringResult(nameof(ctx.AreaOfInfluence), message);
                currentNode.AddResult(error);
            }

            if (Double.IsNaN(ctx.VolumeOfInfluence) || 
                ctx.VolumeOfInfluence <= 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThan, nameof(ctx.VolumeOfInfluence), 0.0);
                var error = new StringResult(nameof(ctx.VolumeOfInfluence), message);
                currentNode.AddResult(error);
            }

            if (String.IsNullOrWhiteSpace(ctx.HoleState))
            {
                var message = String.Format(_fmtNullEmptyWhitespace, nameof(ctx.HoleState));
                var error = new StringResult(nameof(ctx.HoleState), message);
                currentNode.AddResult(error);
            }

            if (String.IsNullOrWhiteSpace(ctx.HoleUsage))
            {
                var message = String.Format(_fmtNullEmptyWhitespace, nameof(ctx.HoleUsage));
                var error = new StringResult(nameof(ctx.HoleUsage), message);
                currentNode.AddResult(error);
            }

            if (String.IsNullOrWhiteSpace(ctx.LayoutType))
            {
                var message = String.Format(_fmtNullEmptyWhitespace, nameof(ctx.LayoutType));
                var error = new StringResult("LayoutType", message);
                currentNode.AddResult(error);
            }

            if (Double.IsNaN(ctx.DesignDiameter) || 
                ctx.DesignDiameter <= 0.0)
            {
                var message = String.Format(_fmtDoubleGreaterThanEqualTo, "DesignDiameter", 0.0);
                var error = new StringResult("DesignDiameter", message);
                currentNode.AddResult(error);
            }

            if (ctx.DesignBenchCollar != null && 
                Double.IsNaN(ctx.DesignBenchCollar.GetValueOrDefault()))
            {
                var message = String.Format(_fmtDoubleValid, "DesignBenchCollar");
                var error = new StringResult("DesignBenchCollar", message);
                currentNode.AddResult(error);
            }

            if (ctx.DesignBenchToe != null &&
                Double.IsNaN(ctx.DesignBenchToe.GetValueOrDefault()))
            {
                var message = String.Format(_fmtDoubleValid, "DesignBenchToe");
                var error = new StringResult("DesignBenchToe", message);
                currentNode.AddResult(error);
            }

            if (Double.IsNaN(ctx.DesignSubDrill))
            {
                var message = String.Format(_fmtDoubleValid, "DesignSubDrill");
                var error = new StringResult("DesignSubDrill", message);
                currentNode.AddResult(error);
            }

            if (ctx.Accuracy != null && 
                Double.IsNaN(ctx.Accuracy.GetValueOrDefault()))
            {
                var message = String.Format(_fmtDoubleValid, "Accuracy");
                var error = new StringResult("Accuracy", message);
                currentNode.AddResult(error);
            }

            if (ctx.LengthAccuracy != null && 
                Double.IsNaN(ctx.LengthAccuracy.GetValueOrDefault()))
            {
                var message = String.Format(_fmtDoubleValid, "LengthAccuracy");
                var error = new StringResult("LengthAccuracy", message);
                currentNode.AddResult(error);
            }

            if (ctx.ChargeTemplateName != null && String.IsNullOrWhiteSpace(ctx.ChargeTemplateName))
            {
                var message = String.Format(_fmtEmptyWhitespace, "ChargeTemplateName");
                var error = new StringResult("ChargeTemplateName", message);
                currentNode.AddResult(error);
            }

            if (ctx.FragmentSize != null && 
                (Double.IsNaN(ctx.FragmentSize.GetValueOrDefault()) || 
                 ctx.FragmentSize.GetValueOrDefault() <= 0.0))
            {
                var message = String.Format(_fmtDoubleGreaterThan, "FragmentSize", 0.0);
                var error = new StringResult("FragmentSize", message);
                currentNode.AddResult(error);
            }

            if (ctx.PowderFactor!= null &&
                (Double.IsNaN(ctx.PowderFactor.GetValueOrDefault()) || 
                 ctx.PowderFactor.GetValueOrDefault() <= 0.0))
            {
                var message = String.Format(_fmtDoubleGreaterThan, "PowderFactor", 0.0);
                var error = new StringResult("PowderFactor", message);
                currentNode.AddResult(error);
            }

            if (ctx.GeologyCode != null && 
                String.IsNullOrWhiteSpace(ctx.GeologyCode))
            {
                var message = String.Format(_fmtEmptyWhitespace, "GeologyCode");
                var error = new StringResult("GeologyCode", message);
                currentNode.AddResult(error);
            }

            if (ctx.DesignChargeProfile != null && 
                ctx.DesignChargeProfile.Count() == 0)
            {
                var message = String.Format(_fmtCollectionItemCountAtLeast, "DesignChargeProfile", 1);
                var error = new StringResult("DesignChargeProfile", message);
                currentNode.AddResult(error);
            }

            if (ctx.ActualChargeProfile != null && 
                ctx.ActualChargeProfile.Count() == 0)
            {
                var message = String.Format(_fmtCollectionItemCountAtLeast, "ActualChargeProfile", 1);
                var error = new StringResult("ActualChargeProfile", message);
                currentNode.AddResult(error);
            }

            if (ctx.DesignHole == null)
            {
                var message = String.Format(_fmtNull, "DesignHole");
                var error = new StringResult("DesignHole", message);
                currentNode.AddResult(error);
            }

            if (ctx.DesignTrace != null && 
                ctx.DesignTrace.Count() < 2)
            {
                var message = String.Format(_fmtCollectionItemCountAtLeast, "DesignTrace", 2);
                var error = new StringResult("DesignTrace", message);
                currentNode.AddResult(error);
            }

            if (ctx.ActualTrace != null && 
                ctx.ActualTrace.Count() < 2)
            {
                var message = String.Format(_fmtCollectionItemCountAtLeast, "ActualTrace", 2);
                var error = new StringResult("ActualTrace", message);
                currentNode.AddResult(error);
            }

            if (ctx.NameBasedProperties != null)
            {
                IValidatorResult childNode = new NodeResult("NameBasedProperties");
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.NameBasedProperties, ref childNode);
            }

            if (ctx.DesignChargeInfo != null)
            {
                IValidatorResult childNode = new NodeResult("DesignChargeInfo");
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.DesignChargeInfo, ref childNode);
            }

            if (ctx.ActualChargeInfo != null)
            {
                IValidatorResult childNode = new NodeResult("ActualChargeInfo");
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.ActualChargeInfo, ref childNode);
            }

            if (ctx.ActualFragmentation != null)
            {
                IValidatorResult childNode = new NodeResult("ActualFragmentation");
                currentNode.AddResult(childNode);
                
                Owner.Validate(ctx.ActualFragmentation, ref childNode);
            }

            if (ctx.DesignChargeProfile != null)
            {
                int index = 0;
                var childListNode = new NodeResult("DesignChargeProfile");
                currentNode.AddResult(childListNode);

                foreach (var interval in ctx.DesignChargeProfile)
                {
                    if (Settings.MaxErrors > 0 &&
                        result.RootResult.ErrorCount >= Settings.MaxErrors)
                    {
                        break;
                    }

                    IValidatorResult childItemNode = new NodeResult(index.ToString());
                    childListNode.AddResult(childItemNode);

                    Owner.Validate(interval, ref childItemNode);
                    index++;
                }
            }

            if (ctx.ActualChargeProfile != null)
            {
                int index = 0;
                var childListNode = new NodeResult("ActualChargeProfile");
                currentNode.AddResult(childListNode);

                foreach (var interval in ctx.ActualChargeProfile)
                {
                    if (Settings.MaxErrors > 0 &&
                        result.RootResult.ErrorCount >= Settings.MaxErrors)
                    {
                        break;
                    }

                    IValidatorResult childItemNode = new NodeResult(index.ToString());
                    childListNode.AddResult(childItemNode);

                    Owner.Validate(interval, ref childItemNode);
                    index++;
                }
            }

            if (ctx.DesignHole != null)
            {
                IValidatorResult childNode = new NodeResult("DesignHole");
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.DesignHole, ref childNode);
            }

            if (ctx.ActualHole != null)
            {
                IValidatorResult childNode = new NodeResult("ActualHole");
                currentNode.AddResult(childNode);

                Owner.Validate(ctx.ActualHole, ref childNode);
            }

            if (ctx.DesignTrace != null)
            {
                int index = 0;
                var childListNode = new NodeResult("DesignTrace");
                currentNode.AddResult(childListNode);

                foreach (var coordinate in ctx.DesignTrace)
                {
                    if (Settings.MaxErrors > 0 && 
                        result.RootResult.ErrorCount >= Settings.MaxErrors)
                    {
                        break;
                    }

                    IValidatorResult childItemNode = new NodeResult(index.ToString());
                    childListNode.AddResult(childItemNode);

                    Owner.Validate(coordinate, ref childItemNode);
                    index++;
                }
            }

            if (ctx.ActualTrace != null)
            {
                int index = 0;
                var childListNode = new NodeResult("ActualTrace");
                currentNode.AddResult(childListNode);

                foreach (var coordinate in ctx.ActualTrace)
                {
                    if (Settings.MaxErrors > 0 &&
                        result.RootResult.ErrorCount >= Settings.MaxErrors)
                    {
                        break;
                    }

                    IValidatorResult childItemNode = new NodeResult(index.ToString());
                    childListNode.AddResult(childItemNode);

                    Owner.Validate(coordinate, ref childItemNode);
                    index++;
                }
            }

            return result.RootResult.ErrorCount == 0;
        }
    }
}
