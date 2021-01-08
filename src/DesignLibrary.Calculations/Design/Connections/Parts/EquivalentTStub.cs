using System;
using System.Linq;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.Output;

namespace TLS.DesignLibrary.Calculations.Design.Connections.Parts
{
    [HiddenCalculation]
    public class EquivalentTStub : ContextlessCalculation
    {
        public double EffectiveLength1 { get; set; }
        public double EffectiveLength2 { get; set; }
        public double Thickness { get; set; }
        public double YieldStrength { get; set; }
        public double MaterialFactorOfSafety { get; set; } = 1;
        public double InnerFlangeDistance { get; set; } //m
        public double OuterFlangeDistance { get; set; } //n
        public double BoltDesignTensionResistance { get; set; }
        public int NumberOfBolts { get; set; }

        public double Mode1MomentResistance { get; private set; }
        public double Mode2MomentResistance { get; private set; }

        public double Mode1DesignResistance { get; private set; }
        public double Mode1DesignResistanceAlternate { get; private set; }
        public double Mode2DesignResistance { get; private set; }
        public double Mode3DesignResistance { get; private set; }

        public double DesignResistance { get; private set; }
        
        protected override void RunBody(OutputBuilder builder)
        {
            Mode1MomentResistance = 0.25 * EffectiveLength1 * Math.Pow(Thickness, 2) * YieldStrength / MaterialFactorOfSafety;
            // TODO: Implement mode 1 alternate as a double check
            Mode2MomentResistance = 0.25 * EffectiveLength2 * Math.Pow(Thickness, 2) * YieldStrength / MaterialFactorOfSafety;

            Mode1DesignResistance = 4 * Mode1MomentResistance / InnerFlangeDistance;
            Mode2DesignResistance =
                (2 * Mode2MomentResistance + OuterFlangeDistance * NumberOfBolts * BoltDesignTensionResistance) /
                (OuterFlangeDistance + InnerFlangeDistance);
            Mode3DesignResistance = NumberOfBolts * BoltDesignTensionResistance;

            DesignResistance = new double[] {Mode1DesignResistance, Mode2DesignResistance, Mode3DesignResistance}.Min();
        }
    }
}
