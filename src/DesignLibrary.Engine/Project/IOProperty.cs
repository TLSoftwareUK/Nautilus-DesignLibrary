using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;
using EnumsNET;
using Jpp.Common;
using TLS.DesignLibrary.Calculations;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Engine.Project
{
    public class IOProperty : BaseNotify
    {
        private readonly PropertyInfo _backingProperty;
        private readonly Calculation _backingInstance;

        public string Name { get; private set; }
        public string Group { get; private set; }
        public string Description { get; private set; }
        
        public bool Required { get; private set; }
        public bool Valid { get; private set; }
        
        public IOPropertyDataType DataType { get; set; }
        
        public IReadOnlyCollection<string> EnumDescriptions { get; private set; }
        
        [JsonIgnore]
        public Type DatasetType { get; private set; }
        
        [JsonIgnore]
        public DatasetItem? DatasetValue
        {
            get
            {
                return GetDatasetValue();}
            set { SetDatasetValue(value); }
        }

        private void SetDatasetValue(DatasetItem value)
        {
            _backingProperty.SetValue(_backingInstance, value);
            Valid = true;
            OnPropertyChanged(nameof(Value));
        }

        private DatasetItem? GetDatasetValue()
        {
            object readValue = _backingProperty.GetValue(_backingInstance);
            if (readValue == null)
                return null;
            
            return (DatasetItem)readValue;
        }

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
                Required = attr1.Required;
                Valid = !(String.IsNullOrWhiteSpace(Value) && Required);

                var t = Nullable.GetUnderlyingType(_backingProperty.PropertyType);

                if (t == null)
                {
                    t = _backingProperty.PropertyType;
                }

                if (t.IsEnum)
                {
                    DataType = IOPropertyDataType.Enum;
                    List<string> _enumDescriptions = new List<string>();
                    
                    var members = Enums.GetMembers(t);
                    foreach (EnumMember enumMember in members)
                    {
                        _enumDescriptions.Add(enumMember.AsString(EnumFormat.Description));
                    }

                    EnumDescriptions = _enumDescriptions;

                }
                if (typeof(DatasetItem).IsAssignableFrom(t))
                {
                    DataType = IOPropertyDataType.Dataset;
                    DatasetType = t;
                }
                else
                {
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
            }
            
            OutputAttribute attr2 = _backingProperty.GetCustomAttribute<OutputAttribute>();
            if (attr2 != null)
            {
                Name = attr2.FriendlyName;
                Group = attr2.Group;
                Description = attr2.Description;
                Required = false;
            }
        }

        private string GetValue()
        {
            object readValue = _backingProperty.GetValue(_backingInstance);
            if (readValue == null)
                return "";

            if (DataType == IOPropertyDataType.Enum)
            {
                readValue = (int) readValue;
            }

            return readValue.ToString();
        }

        private void SetValue(string value)
        {
            object setValue = value;

            switch (DataType)
            {
                case IOPropertyDataType.Numeric:
                    double convertedValue;
                    if (double.TryParse(value, out convertedValue))
                    {
                        setValue = convertedValue;
                    }
                    else
                    {
                        Valid = false;
                        return;
                    }

                    break;

                case IOPropertyDataType.Enum:
                    
                    Type enumType = Nullable.GetUnderlyingType(_backingProperty.PropertyType);
                    setValue = Enum.ToObject(enumType, int.Parse(value));
                    break;
            }

            _backingProperty.SetValue(_backingInstance, setValue);
            Valid = true;
            OnPropertyChanged(nameof(Value));
        }
    }

    public enum IOPropertyDataType
    {
        Numeric,
        Text,
        Enum,
        Dataset
    }
}
