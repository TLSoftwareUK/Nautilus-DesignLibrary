namespace Jpp.DesignCalculations.Calculations.Attributes
{
    public class InputAttribute : PropertyAttribute
    {
        public bool Required { get;  }

        public InputAttribute(string name, string description, string group, bool required = false, UnitTypes units = UnitTypes.Undefined) : base(name, description, group, units)
        {
            Required = required;
        }

        public InputAttribute(string name, bool required = false, UnitTypes units = UnitTypes.Undefined) : base(name, "", "Inputs", units)
        {
            Required = required;
        }
    }
}
