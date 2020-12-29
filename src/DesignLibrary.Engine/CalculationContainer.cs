﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Jpp.Common;
using Jpp.DesignCalculations.Calculations;
using Jpp.DesignCalculations.Engine.Project;

namespace Jpp.DesignCalculations.Engine
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
