using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Calculations.Analysis.Bars
{
    /// <summary>
    /// Base class for all structural bar elements
    /// </summary>
    public abstract class AbstractBar : ContextualCalculation
    {
        // TODO: Consider how these can be exposed
        public AbstractBarDeflection Deflection { get; protected set; }
        public AbstractBarMoment Moment { get; protected set; }
        public AbstractBarShear Shear { get; protected set; }
        public AbstractBarAxial Axial { get; protected set; }

        [Input("AbstractBar_CrossSection_Name", "AbstractBar_CrossSection_Description", "AbstractBar_CrossSection_Group", true)]
        public CrossSection CrossSection { get; set; }
        [Input("AbstractBar_Material_Name", "AbstractBar_Material_Description", "AbstractBar_Material_Group", true)]
        public Material Material { get; set; }
        
        [Input("AbstractBar_Material_Name", "AbstractBar_Material_Description", "AbstractBar_Material_Group", true)]
        public double Length { get; set; }
        
        public Point3d StartPoint { get; set; }

        public Point3d EndPoint { get; set; }

        [Output("AbstractBar_CombinedUsage_Name", "AbstractBar_CombinedUsage_Description", "AbstractBar_CombinedUsage_Group", UnitTypes.Usage)]
        public double[][] CombinedUsage { get; set; }

        /// <inheritdoc/>
        public override void ContextualRunInit(CalculationContext context)
        {
            base.ContextualRunInit(context);
            Moment.CrossSection = CrossSection;
            Moment.Material = Material;
            Axial.CrossSection = CrossSection;
            Axial.Material = Material;
        }

        /// <inheritdoc/>
        public override void RunCombination(int combinationIndex, Combination combination, CalculationContext context)
        {
            Shear.Run(context);
            Moment.Run(context);
            Axial.Run(context);
            Deflection.Run(context);
        }
    }
}
