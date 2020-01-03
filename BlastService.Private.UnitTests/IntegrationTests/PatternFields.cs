namespace BlastService.Private.Tests.IntegrationTests
{
    public enum PatternFields
    {
        All,
        // Required Fields
        NameBasedProperties,
        Stage,
        FaceAngle,
        SubDrill,
        HoleUsage,
        PatternType,
        Purpose,
        PowderFactor,
        RockFactor,
        RockSG,
        ValidationState,
        DesignBoundary,

        // Optional Fields
        Description,
        Bench,
        Pit,
        Phase,
        Area,
        Volume,
        HoleLength,
        Scoring,
        GeologyCode,
        PatternTemplateName,
        ChargingTemplateName,
        IsElectronic,
        MaxHoleFired,
        MaxWeightFired,
        DesignFragmentation,
        ActualFragmentation,
        ActualBoundary,
        Holes
    }
}
