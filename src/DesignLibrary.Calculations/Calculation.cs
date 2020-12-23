using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jpp.DesignCalculations.Calculations.Attributes;

namespace Jpp.DesignCalculations.Calculations
{
    /// <summary>
    /// Abstract class representing a single calculation
    /// </summary>
    public abstract class Calculation
    {
        /// <summary>
        /// Guid uniquely identifying this calculation instance
        /// </summary>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Human readable calculation name
        /// </summary>
        public string CalculationName { get; protected set; }

        /// <summary>
        /// String referring to the name of the resource file for the output
        /// </summary>
        public string TemplateName { get; protected set; }

        /// <summary>
        /// Human readable description
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Human readable reference to the corresponding design code
        /// </summary>
        public string Code { get; protected set; }

        /// <summary>
        /// Boolean indicating if calculation has run
        /// </summary>
        public bool Calculated { get; set; } = false;

        /// <summary>
        /// Resets all outputs and calculated property to default values
        /// </summary>
        public void ResetCalculation()
        {
            Type t = GetType();
            var outputs = t.GetProperties().Where(p => Attribute.IsDefined(p, typeof(OutputAttribute)));
            foreach (PropertyInfo propertyInfo in outputs)
            {
                propertyInfo.SetValue(this, null);
            }

            Calculated = false;
        }

        /// <summary>
        /// Checks all required inputs are not null
        /// </summary>
        public void VerifyInputs()
        {
            Type t = GetType();
            var outputs = t.GetProperties().Where(p => Attribute.IsDefined(p, typeof(InputAttribute)));
            foreach (PropertyInfo propertyInfo in outputs)
            {
                InputAttribute inputAttribute = propertyInfo.GetCustomAttribute<InputAttribute>(true);
                if (inputAttribute.Required)
                {
                    if (propertyInfo.GetValue(this) == null)
                        throw new ArgumentNullException(inputAttribute.FriendlyName, "Missing value");
                }
            }
        }

        protected virtual void RunBegin(OutputBuilder builder)
        {
            builder.BeginCalculation(this);
            ResetCalculation();
            VerifyInputs();
        }
        
        protected virtual void RunEnd(OutputBuilder builder)
        {
            Calculated = true;
            builder.EndCalculation();
        }
    }
}
