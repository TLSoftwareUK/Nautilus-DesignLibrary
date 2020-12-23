using System;
using System.Collections.ObjectModel;
using Jpp.Common;
using Jpp.DesignCalculations.Calculations;

namespace Jpp.DesignCalculations.Engine
{
    public class Scratchpad : BaseNotify
    {
        public const int TIMEOUT_MINUTES = 15;

        public ObservableCollection<ScratchpadItem> Items { get; set; }

        public ScratchpadItem Current { get; private set; }

        public Scratchpad()
        {
            Items = new ObservableCollection<ScratchpadItem>();
        }

        public Calculation AddToSession(CalculationInfo info)
        {
            if (Current == null || (Current.Modified - DateTime.Now).TotalMinutes > TIMEOUT_MINUTES)
            {
                Current = new ScratchpadItem();
                Items.Add(Current);
            }

            return Current.AddCalculation(info);
        }
    }
}
