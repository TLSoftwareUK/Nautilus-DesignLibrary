using System;

namespace Jpp.DesignCalculations.Calculations.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    class AlternateGroupAttribute : Attribute
    {
        public string Key { get; set; }
        public string OptionGroup { get; set; }

        public AlternateGroupAttribute(string key, string optionGroup)
        {
            Key = key;
            OptionGroup = optionGroup;
        }
    }
}
