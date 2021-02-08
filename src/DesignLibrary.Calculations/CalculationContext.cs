using System.Collections.Generic;
using TLS.DesignLibrary.Calculations.DataTypes;
using TLS.DesignLibrary.Calculations.Output;

namespace TLS.DesignLibrary.Calculations
{
    public class CalculationContext
    {
        /// <summary>
        /// Load cases
        /// </summary>
        public List<LoadCase> LoadCases { get; }

        /// <summary>
        /// Collection of combinations
        /// </summary>
        public List<Combination> Combinations { get; }

        /// <summary>
        /// Integer that defines how many segments to split a bar into.
        /// </summary>
        public int NumberBarSegments { get; set; } = 10;

        public OutputBuilder Output { get; private set; }

        public CalculationContext(List<LoadCase> loadCases, List<Combination> combinations, OutputBuilder output)
        {
            LoadCases = loadCases;
            Combinations = combinations;
            Output = output;
        }
    }
}
