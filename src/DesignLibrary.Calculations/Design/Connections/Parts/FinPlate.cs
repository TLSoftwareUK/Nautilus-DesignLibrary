using System;
using Jpp.DesignCalculations.Calculations.Attributes;
using Jpp.DesignCalculations.Calculations.Properties;

namespace Jpp.DesignCalculations.Calculations.Design.Connections.Parts
{
    [HiddenCalculation]
    class FinPlate : Plate
    {
        public FinPlate()
        {
            CalculationName = Resources.FinPlate_CalculationName;
            Description = Resources.FinPlate_Description;
            Code = Resources.FinPlate_Code;
        }

        public override void RunBody(OutputBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}
