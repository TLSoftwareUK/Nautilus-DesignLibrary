using System;
using Jpp.DesignCalculations.Calculations.Attributes;
using Jpp.DesignCalculations.Calculations.DataTypes;

namespace Jpp.DesignCalculations.Calculations.Design.Connections.Parts
{
    [HiddenCalculation]
    class SupportingColumn : ContextualCalculation
    {
        public double Orientation { get; set; }

        public double Thickness { get; set; }
        public double YieldStrength { get; set; }
        public double MaterialFactorOfSafety { get; set; }
        public double InnerGauge { get; private set; }
        public double PlateLength { get; private set; }
        public int NumberOfBoltRows { get; set; }
        public double RowSpacings { get; set; }
        public double HoleDiameter { get; set; }

        public double MomentResistance { get; private set; }
        public double TensionDesignResistance { get; private set; }

        public override void ContextualRunInit(CalculationContext context)
        {
            base.ContextualRunInit(context);
            CalculateTensionsResistance();
        }

        public override void RunCombination(int combinationIndex, Combination combination, CalculationContext context)
        {
            
        }

        private void CalculateTensionsResistance()
        {
            MomentResistance = 0.25 * Math.Pow(Thickness, 2) * YieldStrength / MaterialFactorOfSafety;
            double beta1 = InnerGauge / PlateLength;
            double eta1 = ((NumberOfBoltRows - 1) * RowSpacings - NumberOfBoltRows / 2 * HoleDiameter) / PlateLength;
            double gamma1 = HoleDiameter / PlateLength;
            TensionDesignResistance = 8 * MomentResistance / (1 - beta1) *
                                      (eta1 + 1.5 * Math.Pow(1 - beta1, 0.5) * Math.Pow(1 - gamma1, 0.5));
        }
    }
}
