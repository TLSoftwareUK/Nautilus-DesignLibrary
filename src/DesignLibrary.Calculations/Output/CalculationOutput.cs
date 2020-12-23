using System;
using System.Collections.Generic;

namespace Jpp.DesignCalculations.Calculations.Output
{
    class CalculationOutput
    {
        public Guid CalculationId { get; private set; }

        public Calculation Calculation { get; private set; }

        public List<CalculationOutput> SubCalcs { get; private set; }

        public Dictionary<string, string> Variables { get; private set; }

        public CalculationOutput(Calculation calc)
        {
            Calculation = calc;
            CalculationId = calc.Id;
            SubCalcs = new List<CalculationOutput>();
            Variables = new Dictionary<string, string>();
        }
    }
}
