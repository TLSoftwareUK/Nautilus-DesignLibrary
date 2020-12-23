using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.DesignCalculations.Calculations.DataTypes
{
    /// <summary>
    /// Class representing the physical properties of a material
    /// </summary>
    public class Material
    {
        public string Name { get; set; }
        public double YoungsModulus { get; set; }
        public double YieldStrength { get; set; }
    }
}
