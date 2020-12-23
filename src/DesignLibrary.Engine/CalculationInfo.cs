using System;
using Jpp.DesignCalculations.Calculations;

namespace Jpp.DesignCalculations.Engine
{
    public class CalculationInfo : BrowserInfo
    {
        public string Description { get; private set; }
        public Type BackingCalculationType { get; private set; }

        public CalculationInfo(Calculation calc) : base(calc.CalculationName)
        {
            Description = calc.Description;
            BackingCalculationType = calc.GetType();
        }
    }
}
