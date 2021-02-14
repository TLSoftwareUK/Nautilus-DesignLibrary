using System;

namespace TLS.DesignLibrary.Calculations.DataTypes
{
    /// <summary>
    /// Load case definition
    /// </summary>
    public class LoadCase
    {
        /// <summary>
        /// Internal Guid handle to identify the load case
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        
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
