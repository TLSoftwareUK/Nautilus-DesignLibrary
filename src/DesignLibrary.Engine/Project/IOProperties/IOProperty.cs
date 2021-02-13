using System;
using System.Reflection;
using Jpp.Common;
using TLS.DesignLibrary.Calculations;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Engine.Project.IOProperties
{
    public abstract class IOProperty : BaseNotify
    {
        protected readonly PropertyInfo _backingProperty;
        protected readonly Calculation _backingInstance;

        public string Name { get; private set; }
        public string Group { get; private set; }
        public string Description { get; private set; }
        
        public bool Required { get; private set; }
        public bool Valid { get; protected set; }

        public string Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        protected IOProperty(PropertyInfo binding, Calculation instance)
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

                var t = Nullable.GetUnderlyingType(_backingProperty.PropertyType);

                if (t == null)
                {
                    t = _backingProperty.PropertyType;
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

            if (attr1 == null && attr2 == null)
                throw new InvalidOperationException();
        }

        protected abstract string GetValue();

        protected abstract void SetValue(string value);

        public static IOProperty CreateInstance(PropertyInfo binding, Calculation instance, IUnitConverter converter)
        {
            InputAttribute attr1 = binding.GetCustomAttribute<InputAttribute>();
            UnitTypes? unitType = null;
            if (attr1 != null)
            {
                unitType = attr1.Units;
            }
            
            OutputAttribute attr2 = binding.GetCustomAttribute<OutputAttribute>();
            if (attr2 != null)
            {
                unitType = attr2.Units;
            }
            
            if (attr1 == null && attr2 == null)
                throw new InvalidOperationException();
            
            var t = Nullable.GetUnderlyingType(binding.PropertyType);

            if (t == null)
            {
                t = binding.PropertyType;
            }

            if (t.IsEnum)
            {
                return new EnumIOProperty(binding, instance);
            }
            if (typeof(DatasetItem).IsAssignableFrom(t))
            {
                return new DatasetIOProperty(binding, instance);
            }
            else
            {
                switch (unitType)
                {
                    case UnitTypes.Length:
                    case UnitTypes.Area:
                    case UnitTypes.Pressure:
                    case UnitTypes.Volume:
                    case UnitTypes.LineLoad:
                        return new NumericIOProperty(binding, instance, converter);
                        
                    default:
                        throw new InvalidOperationException("Unsupported datatype");
                }
            }
        }
    }
}
