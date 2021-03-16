using System;
using System.Collections.Generic;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.DataTypes;
using TLS.DesignLibrary.Calculations.Properties;

namespace TLS.DesignLibrary.Calculations.Analysis
{
    public class Panel : ContextualCalculation
    {
        [Input("Panel_Load_Name", "Panel_Loads_Description", "Panel_Loads_Group", true, UnitTypes.Pressure)]
        public Dictionary<Guid, double> Loads { get; set; }
        
        [Input("Panel_HorizontalSpan_Name", "Panel_HorizontalSpan_Description", "Panel_HorizontalSpan_Group", true, UnitTypes.Length)]
        public double HorizontalSpan { get; set; }
        
        [Input("Panel_VerticalSpan_Name", "Panel_VerticalSpan_Description", "Panel_VerticalSpan_Group", true, UnitTypes.Length)]
        public double VerticalSpan { get; set; }
        
        [Input("Panel_TwoWaySpanning_Name", "Panel_TwoWaySpanning_Description", "Panel_TwoWaySpanning_Group", true)]
        public bool TwoWaySpanning { get; set; }
        //TODO: Change to span direction??

        [Output("Panel_HorizontalLineLoad_Name", "Panel_HorizontalLineLoad_Description", "Panel_HorizontalLineLoad_Group", UnitTypes.LineLoad)]
        public Dictionary<Guid, double> HorizontalLineLoad { get; private set; }
        
        [Output("Panel_VerticalLineLoad_Name", "Panel_VerticalLineLoad_Description", "Panel_VerticalLineLoad_Group", UnitTypes.LineLoad)]
        public Dictionary<Guid, double> VerticalLineLoad { get; private set; }
        
        public Panel() : base()
        {
            CalculationName = Resources.Panel_CalculationName;
            Description = Resources.Panel_Description;
            Code = Resources.Panel_Code;
        }
        
        public override void RunCombination(int combinationIndex, Combination combination, CalculationContext context)
        {
            double load = Loads[combination.Id];
            
            if (TwoWaySpanning)
            {
                throw new NotImplementedException();
            }
            else
            {
                HorizontalLineLoad[combination.Id] = 0;
                VerticalLineLoad[combination.Id] = load * HorizontalSpan / 2;
            }
        }
    }
}
