using System;
using TLS.DesignLibrary.Calculations;

namespace TLS.DesignLibrary.Engine
{
    public class ScratchpadItem
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public CalculationContainer Calculations { get; set; }

        public ScratchpadItem()
        {
            Created = DateTime.Now;
            Calculations = new CalculationContainer();
        }

        public Calculation AddCalculation(CalculationInfo info)
        {
            Modified = DateTime.Now;
            //return Calculations.AddCalculation(info);
            return null;
        }
    }
}
