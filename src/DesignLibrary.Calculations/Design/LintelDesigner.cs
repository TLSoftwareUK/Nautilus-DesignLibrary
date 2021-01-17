using System;
using System.Collections.Generic;
using System.Text;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.DataTypes;
using TLS.DesignLibrary.Calculations.Properties;

namespace TLS.DesignLibrary.Calculations.Design
{
    public class LintelDesigner : ContextualCalculation
    {
        [Input("LintelDesigner_Product_Name", 
            "LintelDesigner_Product_Description", 
            "LintelDesigner_Product_Group", true)]
        public Lintel? Product { get; set; }
        
        public LintelDesigner() : base()
        {
            CalculationName = Resources.LintelDesigner_CalculationName;
            Description = Resources.LintelDesigner_Description;
            Code = Resources.LintelDesigner_Code;
        }

        public override void RunCombination(int combinationIndex, Combination combination, CalculationContext context)
        {
            
        }
    }
}
