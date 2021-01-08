using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jpp.Common;
using TLS.DesignLibrary.Calculations;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Engine
{
    public class CalculationViewModel : BaseNotify
    {
        public Calculation Calcuation { get; private set; }

        public string Name
        {
            get { return Calcuation.CalculationName; }
        }

        public string Description
        {
            get { return Calcuation.Description; }
        }

        /// <summary>
        /// Index of active combination
        /// </summary>
        public int CurrentCombination
        {
            get { return _currentCombination; }
            set { SetField(ref _currentCombination, value, nameof(CurrentCombination)); }
        }

        private int _currentCombination;

        public CalculationContext Context { get; set; }

        public Dictionary<string, CalculationPropertyGroup> InputGroups { get; private set; }
        public Dictionary<string, CalculationPropertyGroup> OutputGroups { get; private set; }

        public CalculationViewModel(Calculation calc)
        {
            Calcuation = calc;
            Context = new CalculationContext();
            Context.Combinations.Add(new Combination() { Name = "1" });

            GetInputs();
            GetOutputs();
        }

        private void GetInputs()
        {
            GetProperties(typeof(InputAttribute), InputGroups);
        }

        private void GetOutputs()
        {
            GetProperties(typeof(OutputAttribute), OutputGroups);
        }

        private void GetProperties(Type searchAttribute, Dictionary<string, CalculationPropertyGroup> group)
        {
            group = new Dictionary<string, CalculationPropertyGroup>();
            var properties = Calcuation.GetType().GetProperties()
                .Where(prop => prop.IsDefined(searchAttribute, false));

            foreach (PropertyInfo propertyInfo in properties)
            {
                var attribute = (PropertyAttribute)propertyInfo.GetCustomAttributes(searchAttribute, false).First();
                CalculationProperty newProperty = new CalculationProperty()
                {
                    Name = attribute.FriendlyName,
                    Descritpion = attribute.Description,
                    Group = attribute.Group,
                    Property = propertyInfo
                };
                if (!group.ContainsKey(attribute.Group))
                {
                    CalculationPropertyGroup propertyGroup = new CalculationPropertyGroup()
                    {
                        Name = newProperty.Group
                    };

                    group.Add(newProperty.Group, propertyGroup);
                }

                group[newProperty.Group].Properties.Add(newProperty);
            }
        }

        public void AddCombination()
        {
            Combination comb = new Combination();
            comb.Name = (Context.Combinations.Count + 1).ToString();
            Context.Combinations.Add(comb);

            List<CalculationPropertyGroup> propList = new List<CalculationPropertyGroup>();
            propList.AddRange(InputGroups.Values);
            propList.AddRange(OutputGroups.Values);

            foreach (CalculationPropertyGroup calculationPropertyGroup in propList)
            {
                foreach (CalculationProperty calculationProperty in calculationPropertyGroup.Properties)
                {
                    if (calculationProperty.Property.PropertyType == typeof(List<double>))
                    {
                        List<double> values = calculationProperty.Property.GetValue(Calcuation) as List<double>;
                        values.Add(0);
                    }
                }
            }

            CurrentCombination = Context.Combinations.Count - 1;
        }
    }

    public class CalculationPropertyGroup
    {
        public string Name { get; set; }
        public List<CalculationProperty> Properties { get; set; } = new List<CalculationProperty>();
    }

    public class CalculationProperty
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Descritpion { get; set; }
        public PropertyInfo Property { get; set; }
    }
}
