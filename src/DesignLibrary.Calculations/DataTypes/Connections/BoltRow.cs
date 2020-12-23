using System;
using System.Linq;
using Jpp.DesignCalculations.Calculations.Attributes;
using Jpp.DesignCalculations.Calculations.Design.Connections.Parts;

namespace Jpp.DesignCalculations.Calculations.DataTypes.Connections
{
    [HiddenCalculation]
    // TODO: Only one outstand row is supported at the moment
    public class BoltRow : ContextlessCalculation
    {
        public double Member1Thickness { get; set; }
        public double Member1YieldStrength { get; set; }

        public double MajorDistanceAbove { get; set; }
        public double MajorDistanceBelow { get; set; }
        public double MinorDistanceLeft { get; set; }
        public double MinorDistanceRight { get; set; }
        public double InternalGauge { get; set; }

        public bool CentralStiffener { get; set; }
        public bool TopStiffener { get; set; }
        public bool BottomStiffener { get; set; }

        public int NumberOfBolts { get; set; }
        public Bolt Bolt { get; set; }

        public double CircularEffectiveLength { get; private set; }
        public double NonCircularEffectiveLength { get; private set; }
        public double GroupCircularEffectiveLength { get; private set; }
        public double GroupNonCircularEffectiveLength { get; private set; }
        public double M { get; private set; }
        public double N { get; private set; }

        public double ShearResistance { get; private set; }
        public double Member1MajorBearingResistance { get; private set; }
        public double Member2MajorBearingResistance { get; private set; }

        public double Member1MinorBearingResistance { get; private set; }
        public double Member2MinorBearingResistance { get; private set; }
        public double TensionResistance { get; private set; }

        public override void RunBody(OutputBuilder builder)
        {
            // TODO: Link variables including tension
            
            Member1MajorBearingResistance = NumberOfBolts * Bolt.Member1MajorBearingResistance;
            Member2MajorBearingResistance = NumberOfBolts * Bolt.Member2MajorBearingResistance;
            Member1MinorBearingResistance = NumberOfBolts * Bolt.Member1MinorBearingResistance;
            Member2MinorBearingResistance = NumberOfBolts * Bolt.Member2MinorBearingResistance;
            ShearResistance = NumberOfBolts * Bolt.ShearResistance;

            DetermineEffectiveLength();
            CalculateTensionResistance(builder);

        }

        private void DetermineEffectiveLength()
        {
            if (CentralStiffener)
            {
                // TODO: Add cases with stiffener
                throw new NotImplementedException();
            }
            else
            {
                if (TopStiffener && BottomStiffener)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    if (TopStiffener || BottomStiffener)
                    {
                        if (TopStiffener)
                        {
                            DetermineEdgeOutstandEffectiveLength(MajorDistanceBelow, MajorDistanceAbove);
                        }
                        else
                        {
                            DetermineEdgeOutstandEffectiveLength(MajorDistanceAbove, MajorDistanceBelow);
                        }
                    }
                    else
                    {
                        DetermineOutstandEffectiveLength();
                    }
                }

            }
            
        }

        /// <summary>
        /// Determine effective lengths for an outstand bolt row away from flanges/plate edges
        /// </summary>
        private void DetermineOutstandEffectiveLength()
        {
            throw new NotImplementedException();
        }

        private void DetermineEdgeOutstandEffectiveLength(double distanceAbove, double distanceBelow)
        {
            CircularEffectiveLength = new double[]
            {
                2 * Math.PI * distanceBelow,
                Math.PI * distanceBelow + 2 * distanceAbove,
                Math.PI * distanceBelow + InternalGauge
            }.Min();

            NonCircularEffectiveLength = new double[]
            {
                (MinorDistanceLeft + MinorDistanceRight + InternalGauge)/2,
                4 * distanceBelow + 1.25 * distanceAbove,
                2 * distanceBelow + 0.625 * distanceAbove + (MinorDistanceLeft + MinorDistanceRight)/2,
                2 * distanceBelow + 0.625 * distanceAbove + InternalGauge/2,
            }.Min();

            M = distanceBelow;
            N = Math.Min(distanceAbove, 1.25 * M);
        }

        private void CalculateTensionResistance(OutputBuilder builder)
        {
            EquivalentTStub tStub = new EquivalentTStub();

            tStub.NumberOfBolts = 2; // TODO: Change this to actual number
            tStub.Thickness = Member1Thickness;
            tStub.YieldStrength = Member1YieldStrength;
            tStub.EffectiveLength1 = Math.Min(CircularEffectiveLength, NonCircularEffectiveLength);
            tStub.EffectiveLength2 = NonCircularEffectiveLength;
            tStub.InnerFlangeDistance = M;
            tStub.OuterFlangeDistance = N;
            tStub.BoltDesignTensionResistance = Bolt.TensionResistance;

            tStub.Run(builder);
            TensionResistance = tStub.DesignResistance;
        }
    }
}
