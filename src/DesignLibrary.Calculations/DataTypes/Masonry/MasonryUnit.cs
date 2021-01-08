namespace TLS.DesignLibrary.Calculations.DataTypes.Masonry
{
    public class MasonryUnit
    {
        public double NormalisedMeanCompressiveStrength { get; set; }
    }

    public enum MasonryType
    {
        Clay,
        CalciumSilicate,
        AggregateConcrete,
        AutoclavedAeratedConcrete,
        ManufacturedStone,
        DimensionedNaturalStone
    }
}
