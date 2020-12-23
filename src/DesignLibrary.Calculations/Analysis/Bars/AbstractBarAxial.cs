using Jpp.DesignCalculations.Calculations.DataTypes;

namespace Jpp.DesignCalculations.Calculations.Analysis.Bars
{
    public abstract class AbstractBarAxial : ContextualCalculation
    {
        public CrossSection CrossSection { get; set; }
        public Material Material { get; set; }

        public double[][] Axial { get; set; }

        public double[][] Usage { get; protected set; }
    }
}
