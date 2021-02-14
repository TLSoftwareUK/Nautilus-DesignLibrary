using System;
using System.Collections.Generic;

namespace TLS.DesignLibrary.Calculations.DataTypes
{
    /// <summary>
    /// Represents a single code combination
    /// </summary>
    public class Combination
    {
        /// <summary>
        /// Internal Guid handle to identify the combination
        /// </summary>
        public Guid Id { get; set; }  = Guid.NewGuid();
        
        /// <summary>
        /// Combination name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of combination respresented
        /// </summary>
        public CombinationType CombinationType { get; set; }

        /// <summary>
        /// Array of Partial Factor of Safety to be used
        /// </summary>
        public Dictionary<Guid, double> LoadFactor { get; set; }
    }
}
