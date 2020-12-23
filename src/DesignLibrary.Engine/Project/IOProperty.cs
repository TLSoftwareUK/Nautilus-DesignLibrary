using System.Reflection;
using Jpp.DesignCalculations.Calculations;
using Jpp.DesignCalculations.Calculations.Attributes;

namespace Jpp.DesignCalculations.Engine.Project
{
    public class IOProperty
    {
        private readonly PropertyInfo _backingProperty;
        private readonly Calculation _backingInstance;

        public string Name { get; private set; }
        public string Group { get; private set; }
        public string Description { get; private set; }
        
        public IOPropertyDataType DataType { get; set; }

        public string Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }
        
        public IOProperty(PropertyInfo binding, Calculation instance)
        {
            _backingProperty = binding;
            _backingInstance = instance;

            InputAttribute attr1 = _backingProperty.GetCustomAttribute<InputAttribute>();
            if (attr1 != null)
            {
                Name = attr1.FriendlyName;
                Group = attr1.Group;
                Description = attr1.Description;

                switch (attr1.Units)
                {
                    case UnitTypes.Length: 
                    case UnitTypes.Area:
                    case UnitTypes.Pressure:
                    case UnitTypes.Volume:
                        DataType = IOPropertyDataType.Numeric;
                        break;
                    
                    default:
                        DataType = IOPropertyDataType.Numeric;
                        break;
                }
            }
            
            OutputAttribute attr2 = _backingProperty.GetCustomAttribute<OutputAttribute>();
            if (attr2 != null)
            {
                Name = attr2.FriendlyName;
                Group = attr2.Group;
                Description = attr2.Description;
            }
        }

        private string GetValue()
        {
            object readValue = _backingProperty.GetValue(_backingInstance);
            if (readValue == null)
                return "";

            return readValue.ToString();
        }

        private void SetValue(string value)
        {
            object setValue = value;

            if (DataType == IOPropertyDataType.Numeric)
                setValue = double.Parse(value);
            
            _backingProperty.SetValue(_backingInstance, setValue);
        }
    }

    public enum IOPropertyDataType
    {
        Numeric,
        Text
    }
}
