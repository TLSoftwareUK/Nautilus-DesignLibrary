using System;
using System.Collections.Generic;
using System.Reflection;
using EnumsNET;
using TLS.DesignLibrary.Calculations;

namespace TLS.DesignLibrary.Engine.Project.IOProperties
{
    public class EnumIOProperty : IOProperty
    {
        public IReadOnlyCollection<string> EnumDescriptions { get; private set; }
        
        public EnumIOProperty(PropertyInfo binding, Calculation instance) : base(binding, instance)
        {
            var t = Nullable.GetUnderlyingType(_backingProperty.PropertyType);

            if (t == null)
            {
                t = _backingProperty.PropertyType;
            }
            
            List<string> _enumDescriptions = new List<string>();
                    
            var members = Enums.GetMembers(t);
            foreach (EnumMember enumMember in members)
            {
                _enumDescriptions.Add(enumMember.AsString(EnumFormat.Description));
            }

            EnumDescriptions = _enumDescriptions;
        }

        protected override string GetValue()
        {
            object readValue = _backingProperty.GetValue(_backingInstance);
            if (readValue == null)
                return "";

            readValue = (int) readValue;

            return readValue.ToString();
        }

        protected override void SetValue(string value)
        {
            object setValue = value;

            Type enumType = Nullable.GetUnderlyingType(_backingProperty.PropertyType);
            setValue = Enum.ToObject(enumType, int.Parse(value));

            _backingProperty.SetValue(_backingInstance, setValue);
            Valid = true;
            OnPropertyChanged(nameof(Value));
        }
    }
}
