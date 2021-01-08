using System;
using TLS.DesignLibrary.Calculations;

namespace TLS.DesignLibrary.Engine
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
