using System.ComponentModel;

namespace TLS.DesignLibrary.Calculations.DataTypes
{
    /// <summary>
    /// Load case types
    /// </summary>
    public enum LoadCaseType
    {
        [Description("Permanent")]
        Permanent,
        [Description("Variable")]
        Variable,
        [Description("Wind")]
        Wind,
        [Description("Snow")]
        Snow
    }
}