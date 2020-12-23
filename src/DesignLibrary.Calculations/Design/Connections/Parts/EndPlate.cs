using System;
using Jpp.DesignCalculations.Calculations.Attributes;
using Jpp.DesignCalculations.Calculations.DataTypes;

namespace Jpp.DesignCalculations.Calculations.Design.Connections.Parts
{
    [HiddenCalculation]
    // TODO: Verify incoming for shear??
    public class EndPlate : Plate
    {
        public ICrossSection IncomingCrossSection { get; set; }
        public Material IncomingMaterial { get; set; }

        public double RequiredWeldThroatThickness { get; private set; }

        public override void RunBody(OutputBuilder builder)
        {
            CalculateRequiredWeld();
        }

        private void CalculateRequiredWeld()
        {
            double modificationFactor = 0;
            switch (IncomingMaterial.Name)
            {
                case "S275":
                    modificationFactor = 0.4;
                    break;

                case "S355":
                    modificationFactor = 0.48;
                    break;

                default:
                    throw new InvalidOperationException("Unknown material");
            }

            RequiredWeldThroatThickness = modificationFactor * IncomingCrossSection.WebThickness;
        }
    }
}
