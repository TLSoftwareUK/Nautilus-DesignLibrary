using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Jpp.Common;
using TLS.DesignLibrary.Calculations;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Engine.Project.IOProperties;

namespace TLS.DesignLibrary.Engine.Project
{
    public class EngineCalculation : BaseNotify
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
                    return Status.WaitingCalc;
                }
                catch (ArgumentNullException)
                {
                    return Status.InputRequired;
                }
            }
        }

        [JsonIgnore]
        public IReadOnlyList<IOProperty> Inputs { get; private set; }
        
        [JsonIgnore]
        public IReadOnlyList<IOProperty> Outputs { get; private set; }

        //TODO: Review this and see if can be removed
        [JsonConstructor]
        public EngineCalculation(double x, double y, string instanceName, Calculation calc)
        {
            X = x;
            Y = y;
            InstanceName = instanceName;
            Calc = calc;
        }
        
        public EngineCalculation(double x, double y, string instanceName, Calculation calc, IUnitConverter converter)
        {
            X = x;
            Y = y;
            InstanceName = instanceName;
            Calc = calc;

            BuildProperties(converter);
        }

        private void BuildProperties(IUnitConverter converter)
        {
            List<IOProperty> inputs = new List<IOProperty>();
            List<IOProperty> outputs = new List<IOProperty>();
            
            var inputFields = Calc.GetType().GetProperties()
                .Where(prop => prop.IsDefined(typeof(InputAttribute), false));

            foreach (PropertyInfo prop in inputFields)
            {
                IOProperty newProp = IOProperty.CreateInstance(prop, Calc, converter);
                inputs.Add(newProp);
                newProp.PropertyChanged += (sender, args) => { OnPropertyChanged(nameof(Calc)); };
            }
            
            var outputFields = Calc.GetType().GetProperties()
                .Where(prop => prop.IsDefined(typeof(OutputAttribute), false));

            foreach (PropertyInfo prop in outputFields)
            {
                outputs.Add(IOProperty.CreateInstance(prop, Calc, converter));
            }

            Inputs = inputs.OrderBy(a => a.Group).ThenBy(a => a.Name).ToList();
            Outputs = outputs.OrderBy(a => a.Group).ThenBy(a => a.Name).ToList();
        }

        public void OnDeserialize(IUnitConverter converter)
        {
            BuildProperties(converter);
            
            foreach (IOProperty input in Inputs)
            {
                input.PropertyChanged += (sender, args) =>
                {
                    this.Calc.Calculated = false;
                    this.OnPropertyChanged(nameof(Inputs));
                };
            }
        }
    }

    public enum Status
    {
        Ok,
        Failed,
        Warning,
        InputRequired,
        WaitingCalc
    }
    
}


