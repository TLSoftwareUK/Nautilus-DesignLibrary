using System;
using System.Collections.Generic;
using System.Text;

namespace TLS.DesignLibrary.Calculations.DataTypes
{
    public class Lintel : DatasetItem
    {
        public float Length { get; set; }
        
        public float MinimumCavity { get; set; }
        public float MaximumCavity { get; set; }
        
        public float MinimumInnerLeaf { get; set; }
        public float MaximumInnerLeaf { get; set; }
        
        public float Weight { get; set; }
        public float TotalUDL { get; set; }
        
        public float? Price { get; set; }
    }
}
