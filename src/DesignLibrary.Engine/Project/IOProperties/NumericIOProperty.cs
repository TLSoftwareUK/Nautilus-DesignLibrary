using System;
using System.Reflection;
using System.Text.Json.Serialization;
using TLS.DesignLibrary.Calculations;
using TLS.DesignLibrary.Calculations.Attributes;

namespace TLS.DesignLibrary.Engine.Project.IOProperties
{
    public class NumericIOProperty : IOProperty
    {
        [JsonIgnore]
        private IUnitConverter _converter;

        public UnitTypes UnitType { get; private set; }

        public string UnitText { get; set; }

        public NumericIOProperty(PropertyInfo binding, Calculation instance, IUnitConverter converter) : base(binding, instance)
        {
            _converter = converter;

            InputAttribute attr1 = binding.GetCustomAttribute<InputAttribute>();
            if (attr1 != null)
            {
                UnitType = attr1.Units;
            }

            OutputAttribute attr2 = binding.GetCustomAttribute<OutputAttribute>();
            if (attr2 != null)
            {
                UnitType = attr2.Units;
            }

            if (attr1 == null && attr2 == null)
                throw new InvalidOperationException();

            UnitText = _converter.GetUnits(UnitType);
            Valid = !(String.IsNullOrWhiteSpace(Value) && Required);
        }

        protected override string GetValue()
        {
            object readValue = _backingProperty.GetValue(_backingInstance);
            if (readValue == null)
                return "";

            readValue = (double)readValue * _converter.GetConversion(UnitType);
            return readValue.ToString();
        }

        protected override void SetValue(string value)
        {
            object setValue = value;

            double convertedValue;
            if (double.TryParse(value, out convertedValue))
            {
                setValue = convertedValue / _converter.GetConversion(UnitType);
            }
            else
            {
                Valid = false;
                return;
            }

            _backingProperty.SetValue(_backingInstance, setValue);
            Valid = true;
            OnPropertyChanged(nameof(Value));
        }
    }
}
