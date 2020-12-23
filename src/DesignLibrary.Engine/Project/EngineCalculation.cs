using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jpp.DesignCalculations.Calculations;
using Jpp.DesignCalculations.Calculations.Attributes;

namespace Jpp.DesignCalculations.Engine.Project
{
    public class EngineCalculation
    {
        public double X { get; set; }
        public double Y { get; set; }

        public string InstanceName { get; set; }

        public Calculation Calc { get; set; }

        public Status Status
        {
            get
            {
                if (Calc.Calculated)
                    return Status.Ok;

                try
                {
                    Calc.VerifyInputs();
                    return Status.Failed;
                }
                catch (ArgumentNullException)
                {
                    return Status.InputRequired;
                }
            }
        }

        public IReadOnlyList<IOProperty> Inputs { get; private set; }
        public IReadOnlyList<IOProperty> Outputs { get; private set; }

        public EngineCalculation(double x, double y, string friendlyName, Calculation instance)
        {
            X = x;
            Y = y;
            InstanceName = friendlyName;
            Calc = instance;

            List<IOProperty> inputs = new List<IOProperty>();
            List<IOProperty> outputs = new List<IOProperty>();
            
            var inputFields = Calc.GetType().GetProperties()
                .Where(prop => prop.IsDefined(typeof(InputAttribute), false));

            foreach (PropertyInfo prop in inputFields)
            {
                inputs.Add(new IOProperty(prop, instance));
            }
            
            var outputFields = Calc.GetType().GetProperties()
                .Where(prop => prop.IsDefined(typeof(OutputAttribute), false));

            foreach (PropertyInfo prop in outputFields)
            {
                outputs.Add(new IOProperty(prop, instance));
            }

            Inputs = inputs.OrderBy(a => a.Group).ThenBy(a => a.Name).ToList();
            Outputs = outputs.OrderBy(a => a.Group).ThenBy(a => a.Name).ToList();
        }
    }

    public enum Status
    {
        Ok,
        Failed,
        Warning,
        InputRequired
    }
    
}


