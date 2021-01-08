using System;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.Output;
using TLS.DesignLibrary.Calculations.Properties;

namespace TLS.DesignLibrary.Calculations.Design.Connections.Parts
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

        protected override void RunBody(OutputBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}
