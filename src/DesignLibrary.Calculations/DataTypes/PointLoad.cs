using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.DesignCalculations.Calculations.DataTypes
{
    public class PointLoad
    {
        public Point3d Location { get; set; }
        public double XForce { get; set; }
        public double YForce { get; set; }
    }
}

