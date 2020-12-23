using System;
using System.Collections.Generic;
using Jpp.Common;
using Jpp.DesignCalculations.Calculations;
using Jpp.DesignCalculations.Engine.Project;

namespace Jpp.DesignCalculations.Engine
{
    public class CalculationContainer : BaseNotify
    {
        public IReadOnlyList<EngineCalculation> ContextlessCalculations
        {
            get { return _contextlessCalculations; }
        }

        private List<EngineCalculation> _contextlessCalculations;

        public IReadOnlyList<EngineCalculation> ContextualCalculations
        {
            get { return _contextualCalculations; }
        }

        private List<EngineCalculation> _contextualCalculations;

        public CalculationContainer()
        {
            _contextlessCalculations = new List<EngineCalculation>();
            _contextualCalculations = new List<EngineCalculation>();
        }

        public Calculation AddCalculation(CalculationInfo info, double x, double y, string name)
        {
            Calculation calc = (Calculation)Activator.CreateInstance(info.BackingCalculationType);
            EngineCalculation eCalc = new EngineCalculation(x, y, name, calc);
            if (typeof(ContextualCalculation).IsAssignableFrom(calc.GetType()))
            {
                _contextualCalculations.Add(eCalc);   
                OnPropertyChanged(nameof(ContextualCalculations));
            }
            else
            {
                _contextlessCalculations.Add(eCalc);
                OnPropertyChanged(nameof(ContextlessCalculations));
            }

            return calc;
        }
    }
}
