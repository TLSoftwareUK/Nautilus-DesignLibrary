using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Calculations.Analysis.Bars
{
    public abstract class AbstractBarMoment : ContextualCalculation
    {
        public CrossSection CrossSection { get; set; }
        public Material Material { get; set; }

        public double[][] MajorMoment { get; set; }
        public double[][] MinorMoment { get; set; }

        public double MajorMomentResistance { get; protected set; }
        public double MinorMomentResistance { get; protected set; }

        public double[][] MajorUsage { get; set; }
        public double[][] MinorUsage { get; set; }
    }
}
