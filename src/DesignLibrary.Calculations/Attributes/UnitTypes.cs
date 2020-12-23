using System;

namespace Jpp.DesignCalculations.Calculations.Attributes
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
        Pressure
    }
}
