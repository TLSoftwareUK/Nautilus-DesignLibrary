namespace TLS.DesignLibrary.Calculations.DataTypes
{
    /// <summary>
    /// Load case definition
    /// </summary>
    public class LoadCase
    {
        /// <summary>
        /// User readable name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of load case
        /// </summary>
        public LoadCaseType Type { get; set; }
    }
}
