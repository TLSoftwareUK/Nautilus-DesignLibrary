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
            object readValue;
            
            if (Indexed)
            {
                PropertyInfo pInfo =  _backingProperty.GetValue(_backingInstance).GetType().GetProperty("Item", new[] {typeof(int) } );
                readValue = pInfo.GetValue(new int[] {Index});

            }
            else
            {
                readValue = _backingProperty.GetValue(_backingInstance);
            }
            
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

            //_backingProperty.SetValue(_backingInstance, setValue);
            
            if (Indexed)
            {
                PropertyInfo pInfo =  _backingProperty.GetValue(_backingInstance).GetType().GetProperty("Item", new[] {typeof(int) } );
                pInfo.SetValue(_backingInstance, setValue, new object[] {Index});

            }
            else
            {
                _backingProperty.SetValue(_backingInstance, setValue);
            }
            
            Valid = true;
            OnPropertyChanged(nameof(Value));
        }
    }
}
