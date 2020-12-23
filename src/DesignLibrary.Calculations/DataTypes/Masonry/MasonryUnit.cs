using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.DesignCalculations.Calculations.DataTypes.Masonry
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
