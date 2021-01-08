using System;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Calculations.Analysis.Bars.Steel
{
    [HiddenCalculation]
    public class SteelBar : AbstractBar
    {
        public int SectionClassification { get; private set; }

        public SteelBar()
        {
            Moment = new SteelBarMoment();
            Axial = new SteelBarAxial();
        }

        public override void ContextualRunInit(CalculationContext context)
        {
            CombinedUsage = new double[context.Combinations.Count][];
            if (SectionClassification > 3)
            {
                throw new NotImplementedException();
            }
        }

        public override void RunCombination(int combinationIndex, Combination combination, CalculationContext context)
        {
            // Clause 6.2.1 (7)
            CombinedUsage[combinationIndex] = new double[context.NumberBarSegments + 1];
            for (int i = 0; i <= context.NumberBarSegments; i++)
            {
                // Eq 6.2
                CombinedUsage[combinationIndex][i] = Axial.Usage[combinationIndex][i] +
                                                     Moment.MajorUsage[combinationIndex][i] +
                                                     Moment.MinorUsage[combinationIndex][i];
            }
        }
    }
}
