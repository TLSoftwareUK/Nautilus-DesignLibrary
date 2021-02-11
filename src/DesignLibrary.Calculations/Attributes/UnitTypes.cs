using System;

namespace TLS.DesignLibrary.Calculations.Attributes
{
    public enum UnitTypes
    {
        [Obsolete("Undefined should not be used, please specify a unit")]
        Undefined,
        Text,
        Usage,
        Length,
        Area,
        Volume,
        Pressure,
        LineLoad
    }
}
