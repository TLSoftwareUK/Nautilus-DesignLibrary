using System;
using System.Collections.Generic;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.DataTypes;
using TLS.DesignLibrary.Calculations.Properties;

namespace TLS.DesignLibrary.Calculations.Components
{
    class AreaLoadInput : ContextualCalculation
    {
        [Input("AreaLoadInput_ComponentLoad_Name", "AreaLoadInput_ComponentLoad_Description", "AreaLoadInput_ComponentLoad_Group", true)]
        public List<AreaLoad> ComponentLoads { get; set; }
        
        //[Output("AreaLoadInput_Load_Name", "AreaLoadInput_Load_Description", "AreaLoadInput_Load_Group", UnitTypes.Pressure)]
        public Dictionary<Guid, double> Load { get; set; }
        
        public AreaLoadInput() : base()
        {
            CalculationName = Resources.AreaLoadInput_CalculationName;
            Description = Resources.AreaLoadInput_Description;
            Code = Resources.AreaLoadInput_Code;

            ComponentLoads = new List<AreaLoad>();
            Load = new Dictionary<Guid, double>();
        }
        
        public override void RunCombination(Combination combination, CalculationContext context)
        {
            double result = 0;
            foreach (AreaLoad componentLoad in ComponentLoads)
            {
                result += componentLoad.Magntidue * combination.LoadFactor[componentLoad.Case.Id];
            }

            Load[combination.Id] = result;

        }
    }
}
