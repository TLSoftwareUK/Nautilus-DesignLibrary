using System;
using Jpp.DesignCalculations.Calculations.Attributes;
using Jpp.DesignCalculations.Calculations.DataTypes.Masonry;

namespace Jpp.DesignCalculations.Calculations.Design.Masonry
{
    public class EurocodeCharacteristicCompressiveStrength : ContextlessCalculation
    {
        [AlternateGroup("masonry", "a")]
        public MasonryUnit? MasonryUnit { get; set; }

        [AlternateGroup("masonry", "b")]
        public double NormalisedMeanCompressiveStrength { get; set; }

        public double MortarCompressiveStrength { get; set; }

        [Output("", "", "", UnitTypes.Pressure)]
        public double CharacteristicCompressiveStrength { get; set; }

        public override void RunBody(OutputBuilder builder)
        {
            if (MasonryUnit != null)
            {
                NormalisedMeanCompressiveStrength = MasonryUnit.NormalisedMeanCompressiveStrength;
            }

            CharacteristicCompressiveStrength = K() * Math.Pow(NormalisedMeanCompressiveStrength, Alpha()) *
                                                Math.Pow(MortarCompressiveStrength, Beta());

            Calculated = true;
        }

        private double K()
        {
            return 0;
        }

        private double Alpha()
        {
            return 0;
        }

        private double Beta()
        {
            return 0;
        }
    }
}
