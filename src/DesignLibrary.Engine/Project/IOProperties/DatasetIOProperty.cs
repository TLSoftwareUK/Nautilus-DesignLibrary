using System;
using System.Reflection;
using TLS.DesignLibrary.Calculations;
using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Engine.Project.IOProperties
{
    public class DatasetIOProperty : IOProperty
    {
        public Type DatasetType { get; private set; }
        
        public DatasetItem DatasetValue
        {
            get
            {
                return GetDatasetValue();
            }
            set { SetDatasetValue(value); }
        }

        public DatasetIOProperty(PropertyInfo binding, Calculation instance) : base(binding, instance)
        {
            var t = Nullable.GetUnderlyingType(_backingProperty.PropertyType);

            if (t == null)
            {
                t = _backingProperty.PropertyType;
            }

            DatasetType = t;
        }

        protected override string GetValue()
        {
            throw new NotImplementedException();
        }

        protected override void SetValue(string value)
        {
            throw new NotImplementedException();
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
    }
}
