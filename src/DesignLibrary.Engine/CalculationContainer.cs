using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Jpp.Common;
using TLS.DesignLibrary.Calculations;
using TLS.DesignLibrary.Engine.Project;

namespace TLS.DesignLibrary.Engine
{
    public class CalculationContainer : BaseNotify
    {
        [JsonInclude]
        public List<EngineCalculation> ContextlessCalculations
        {
            get { return _contextlessCalculations; }
            private set { _contextlessCalculations = value; }
        }

        private List<EngineCalculation> _contextlessCalculations;

        [JsonInclude]
        public List<EngineCalculation> ContextualCalculations
        {
            get { return _contextualCalculations; }
            private set { _contextualCalculations = value; }
        }

        public IReadOnlyList<EngineCalculation> Calculations
        {
            get
            {
                List<EngineCalculation> result = new List<EngineCalculation>();
                result.AddRange(ContextlessCalculations);
                result.AddRange(ContextualCalculations);
                return result;
            }
        }

        private List<EngineCalculation> _contextualCalculations;

        public CalculationContainer()
        {
            _contextlessCalculations = new List<EngineCalculation>();
            _contextualCalculations = new List<EngineCalculation>();
        }

        public Calculation AddCalculation(Type info, double x, double y, string name)
        {
            Calculation calc = (Calculation)Activator.CreateInstance(info);
            EngineCalculation eCalc = new EngineCalculation(x, y, name, calc);
            eCalc.PropertyChanged += (sender, args) => { OnPropertyChanged(nameof(ContextlessCalculations)); };
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

        //TODO: Should this have a converter?
        public void OnDeserialize()
        {
            foreach (EngineCalculation calc in Calculations)
            {
                calc.PropertyChanged += (sender, args) => this.OnPropertyChanged(nameof(Calculations));
                calc.OnDeserialize();
            }
        }
    }
}
