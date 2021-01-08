using System;
using TLS.DesignLibrary.Calculations.Design.Connections.Parts;
using TLS.DesignLibrary.Calculations.Properties;

namespace TLS.DesignLibrary.Calculations.Design.Connections
{
    class FinPlateConnection : SteelConnection
    {
        public FinPlateConnection() : base()
        {


            ContextlessSubCalculations.Add(new FinPlate());
            CalculationName = Resources.FinPlateConnection_CalculationName;
            Description = Resources.FinPlateConnection_Description;
            Code = Resources.FinPlateConnection_Code;
        }

        public override bool CheckDetailRequirements()
        {
            throw new NotImplementedException();
        }

        public override void CheckTying()
        {
            throw new NotImplementedException();
        }
    }
}
