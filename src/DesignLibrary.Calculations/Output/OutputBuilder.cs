using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Jpp.DesignCalculations.Calculations.Output;

namespace Jpp.DesignCalculations.Calculations
{
    public class OutputBuilder
    {
        private XElement _outputTree;

        private Stack<CalculationOutput> _currentCalc;

        public OutputBuilder()
        {
            _currentCalc = new Stack<CalculationOutput>();
        }
        
        public void BeginOutput()
        {
            
        }

        public void EndOutput()
        {

        }

        public void Clear()
        {

        }

        public void BeginCalculation(Calculation calc)
        {
            CalculationOutput output = new CalculationOutput(calc);

            if (_currentCalc.Count > 0)
            {
                _currentCalc.Peek().SubCalcs.Add(output);
            }
            _currentCalc.Push(output);
        }

        public void EndCalculation()
        {
            _currentCalc.Pop();
        }

        public void AddVariable(string key, string value)
        {
            if(_currentCalc.Count == 0)
                throw new InvalidOperationException("No active calculation to add to");

            _currentCalc.Peek().Variables.Add(key, value);
        }
    }
}
