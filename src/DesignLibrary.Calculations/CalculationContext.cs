using System.Collections.Generic;
using Jpp.DesignCalculations.Calculations.DataTypes;

namespace Jpp.DesignCalculations.Calculations
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

        public CalculationContext()
        {
            LoadCases = new List<LoadCase>();
            Combinations = new List<Combination>();
            Output = new OutputBuilder();
        }
    }
}
