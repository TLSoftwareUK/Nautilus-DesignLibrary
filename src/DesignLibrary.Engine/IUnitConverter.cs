using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.DesignLibrary.Calculations.Attributes;

namespace TLS.DesignLibrary.Engine
{
    public interface IUnitConverter
    {
        public double PressureConversion { get; }
        public string Pressure { get; }
        
        public double LineLoadConversion { get; }
        public string LineLoad { get; }
        
        public double LengthConversion { get; }
        public string Length { get; }

        public double GetConversion(UnitTypes uType);
        public string GetUnits(UnitTypes uType);
    }
}

