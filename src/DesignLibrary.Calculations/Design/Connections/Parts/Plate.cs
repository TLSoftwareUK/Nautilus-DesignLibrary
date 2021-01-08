using TLS.DesignLibrary.Calculations.Attributes;

namespace TLS.DesignLibrary.Calculations.Design.Connections.Parts
{
    // TODO: Consider block tearing (3.10.2)
    public abstract class Plate : ContextlessCalculation
    {
        [Input("Plate_MajorDimension_Name", "Plate_MajorDimension_Description", "Plate_MajorDimension_Group")]
        public double MajorDimension { get; set; }
        [Input("Plate_MinorDimension_Name", "Plate_MinorDimension_Description", "Plate_MinorDimension_Group")]
        public double MinorDimension { get; set; }

        [Input("Plate_Thickness_Name", "Plate_Thickness_Description", "Plate_Thickness_Group")]
        public double Thickness { get; set; }
    }
}
