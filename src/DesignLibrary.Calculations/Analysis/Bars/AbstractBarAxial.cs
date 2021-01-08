using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Calculations.Analysis.Bars
{
    public abstract class AbstractBarAxial : ContextualCalculation
    {
        public CrossSection CrossSection { get; set; }
        public Material Material { get; set; }

        public double[][] Axial { get; set; }

        public double[][] Usage { get; protected set; }
    }
}
