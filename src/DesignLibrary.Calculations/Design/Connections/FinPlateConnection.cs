using System;
using System.Collections.Generic;
using System.Text;
using Jpp.DesignCalculations.Calculations.Design.Connections.Parts;
using Jpp.DesignCalculations.Calculations.Properties;

namespace Jpp.DesignCalculations.Calculations.Design.Connections
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
