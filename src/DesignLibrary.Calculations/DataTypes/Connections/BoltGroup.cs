using System.Collections.Generic;
using Jpp.DesignCalculations.Calculations.Attributes;

namespace Jpp.DesignCalculations.Calculations.DataTypes.Connections
{
    [HiddenCalculation]
    public class BoltGroup : ContextlessCalculation
    {
        // Frd
        public double ShearResistance { get; private set; }
        public double Member1MajorBearingResistance { get; private set; }
        public double Member2MajorBearingResistance { get; private set; }
        
        public double Member1MinorBearingResistance { get; private set; }
        public double Member2MinorBearingResistance { get; private set; }

        public double TensionResistance { get; private set; }

        public Location RowLocation { get; private set; }

        private List<BoltRow> _rows;

        public BoltGroup()
        {
            _rows = new List<BoltRow>();
        }

        public override void RunBody(OutputBuilder builder)
        {
            ShearResistance = 0;
            Member1MajorBearingResistance = 0;
            Member2MajorBearingResistance = 0;
            foreach (BoltRow boltRow in _rows)
            {
                ShearResistance += boltRow.ShearResistance;
                Member2MajorBearingResistance += boltRow.Member2MajorBearingResistance;
            }

            Calculated = true;
        }

        public enum Location
        {
            TopOutstand,
            BottomOutstand,
            Top,
            Middle,
            Bottom
        }
    }
}
