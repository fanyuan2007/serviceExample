namespace BlastService.Private.ModelContract.Validation
{
    public interface IValidator
    {
        bool Validate(ref IValidatorResult result, object context);
        bool CanValidate(object context);
        IValidationSettings Settings { get; set; }
    }
}
