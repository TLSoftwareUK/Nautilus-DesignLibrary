using System.Collections.Generic;
using System.Linq;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.Output;

namespace TLS.DesignLibrary.Calculations.DataTypes.Connections
{
    [HiddenCalculation]
    // TODO: Add countersunk bolt option
    // TODO: Add code for oversized/slotted holes
    public class Bolt : ContextlessCalculation
    {
        public double Diameter { get; set; }
        public double HoleDiameter { get; set; }
        public double Grade { get; private set; } = 8.8d;
        public double PartialFactorResistanceBolt { get; private set; }= 1.25;

        public double Member1Thickness { get; set; }
        public double Member1UltimateStrength { get; set; }
        public double Member2Thickness { get; set; }
        public double Member2UltimateStrength { get; set; }

        public double? Member1MajorEdgeDistance { get; set; }
        public double? Member2MajorEdgeDistance { get; set; }
        public double? Member1MinorEdgeDistance { get; set; }
        public double? Member2MinorEdgeDistance { get; set; }

        public double? MajorSpacing { get; set; }
        public double? MinorSpacing { get; set; }

        public bool LimitShearByTension { get; set; }

        // Fub
        // 800 for grade 8.8 only 
        public double DesignUltimateStrength { get; private set; } = 800000;
        public double TensileStressArea { get; set; }

        public double ShearResistance { get; private set; }

        public double Member1MajorBearingResistance { get; private set; }
        public double Member2MajorBearingResistance { get; private set; }

        public double Member1MinorBearingResistance { get; private set; }
        public double Member2MinorBearingResistance { get; private set; }

        public double TensionResistance { get; private set; }

        protected override void RunBody(OutputBuilder builder)
        {
            // 0.6 for grade 8.8 and 4.6, 0.5 for class 10.9
            // 0.8 to allow for presence of tension in bolt
            ShearResistance = 0.6 * DesignUltimateStrength * TensileStressArea / PartialFactorResistanceBolt;
            if (LimitShearByTension) // If tensions is present assume its in full use, which reduces the shear down to 28% of its normal value
                ShearResistance = ShearResistance * 0.4d / 1.4d; // 6.2.2 (2)


            Member1MajorBearingResistance = CalculateBearingResistance(Member1MajorEdgeDistance, MajorSpacing,
                Member1MinorEdgeDistance, MinorSpacing, Member1UltimateStrength, Member1Thickness);
            Member2MajorBearingResistance = CalculateBearingResistance(Member2MajorEdgeDistance, MajorSpacing,
                Member2MinorEdgeDistance, MinorSpacing, Member2UltimateStrength, Member2Thickness);
            Member1MinorBearingResistance = CalculateBearingResistance(Member1MinorEdgeDistance, MinorSpacing,
                Member1MajorEdgeDistance, MajorSpacing, Member1UltimateStrength, Member1Thickness);
            Member2MinorBearingResistance = CalculateBearingResistance(Member2MinorEdgeDistance, MinorSpacing,
                Member2MajorEdgeDistance, MajorSpacing, Member2UltimateStrength, Member2Thickness);

            //Table 3.3
            //0.9 when not countersunk, 0.63 otherwise
            TensionResistance = 0.9 * DesignUltimateStrength * TensileStressArea / PartialFactorResistanceBolt;

            Calculated = true;
        }

        private double CalculateBearingResistance(double? majorEdgeDistance, double? majorSpacing, double? minorEdgeDistance, double? minorSpacing, double memberUltimateStrength, double memberThickness)
        {
            List<double> alphaValues = new List<double>()
            {
                DesignUltimateStrength / memberUltimateStrength,
                1d
            };

            if (majorEdgeDistance.HasValue)
            {
                alphaValues.Add(majorEdgeDistance.Value / (3 * HoleDiameter));
            }
            if (majorSpacing.HasValue)
            {
                alphaValues.Add(majorSpacing.Value / (3 * HoleDiameter) - 0.25);
            }

            double Alpha = alphaValues.Min();

            List<double> kappaValues = new List<double>()
            {
                2.5d
            };

            if (minorEdgeDistance.HasValue)
            {
                kappaValues.Add(2.8d * minorEdgeDistance.Value / HoleDiameter - 1.7d);
            }
            if (minorSpacing.HasValue)
            {
                kappaValues.Add(1.4 * minorSpacing.Value / HoleDiameter - 1.7);
            }

            double Kappa = kappaValues.Min();

            return Kappa * Alpha * memberUltimateStrength * Diameter * memberThickness / PartialFactorResistanceBolt;
        }
    }
}
