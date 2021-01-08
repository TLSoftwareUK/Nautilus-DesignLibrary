using System;

namespace TLS.DesignLibrary.Engine
{
    public class Element : CalculationContainer
    {
        public Guid Id { get; private set; }

        public string Name { get; set; }

        public Element()
        {
            Id = Guid.NewGuid();
        }

    }
}
