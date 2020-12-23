namespace Jpp.DesignCalculations.Calculations.Attributes
{
    
    public class OutputAttribute : PropertyAttribute
    {
        public OutputAttribute(string name, string description, string group, UnitTypes units = UnitTypes.Undefined) :
            base(name, description, group, units)
        {

        }

        public OutputAttribute(string name, UnitTypes units = UnitTypes.Undefined) : base(name, "", "Outputs", units)
        {
        }
    }
}
