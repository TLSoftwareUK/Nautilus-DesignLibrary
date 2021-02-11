using System.Collections.Generic;
using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Engine
{
    public class ActiveCombinationGroup
    {
        public string GroupName { get; set; }

        public List<Combination> Combinations { get; set; }

        public ActiveCombinationMode Mode { get; set; }
    }

    public enum ActiveCombinationMode
    {
        Maxima,
        Minima
    }
}
