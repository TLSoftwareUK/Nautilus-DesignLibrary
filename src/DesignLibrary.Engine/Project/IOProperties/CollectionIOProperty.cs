using System;
using System.Collections.ObjectModel;
using System.Reflection;
using TLS.DesignLibrary.Calculations;

namespace TLS.DesignLibrary.Engine.Project.IOProperties
{
    public class CollectionIOProperty : IOProperty
    {
        public ObservableCollection<IOProperty> Properties { get; set; }

        private IUnitConverter _converter;
        
        public CollectionIOProperty(PropertyInfo binding, Calculation instance, IUnitConverter converter) : base(binding, instance)
        {
            Properties = new ObservableCollection<IOProperty>();
            _converter = converter;
        }

        protected override string GetValue()
        {
            throw new NotImplementedException();
        }

        protected override void SetValue(string value)
        {
            throw new NotImplementedException();
        }

        public void Add()
        {
            Type[] generics = _backingProperty.PropertyType.GetGenericArguments();
            //TODO: Check for multiple??
            _backingProperty
             //Activator.CreateInstance(generics[0]);
             Properties IOProperty.CreateCollectionInstance(_backingProperty, _backingInstance, _converter);
        }
    }
}
