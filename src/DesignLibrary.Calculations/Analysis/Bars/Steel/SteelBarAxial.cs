using System;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Calculations.Analysis.Bars.Steel
{
    [HiddenCalculation]
    public class SteelBarAxial : AbstractBarAxial
    {
        public double TensionResistance { get; set; }
        public double CompressionResistance { get; set; }
        public int SectionClassification { get; private set; }

        public double FactorSafety { get; set; } = 1;

        public override void ContextualRunInit(CalculationContext context)
        {
            Usage = new double[context.Combinations.Count][];

            // TODO: Consider holes

            // Eq. 6.6
            TensionResistance = -CrossSection.Area * Material.YieldStrength / FactorSafety;

            if (SectionClassification < 4)
            {
                // Eq 6.10
                CompressionResistance = CrossSection.Area * Material.YieldStrength / FactorSafety;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override void RunCombination(int combinationIndex, Combination combination, CalculationContext context)
        {
            Usage[combinationIndex] = new double[context.NumberBarSegments + 1];
            for (int i = 0; i <= context.NumberBarSegments; i++)
            {
                if (Axial[combinationIndex][i] > 0)
                {
                    Usage[combinationIndex][i] = Axial[combinationIndex][i] / CompressionResistance;
                }
                else
                {
                    Usage[combinationIndex][i] = Axial[combinationIndex][i] / TensionResistance;
                }
            }
        }
    }
}
