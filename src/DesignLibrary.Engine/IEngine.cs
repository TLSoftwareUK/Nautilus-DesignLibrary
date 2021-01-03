﻿using System.ComponentModel;
using System.Threading.Tasks;
using Jpp.DesignCalculations.Calculations;

namespace Jpp.DesignCalculations.Engine
{
    public interface IEngine
    {
        OutputBuilder Output { get; }
        
        IEngineStatus Status { get; }

        void SetContainer(CalculationContainer container);

        Task RunAsync();

        void InvalidateCalculation(Calculation calc);
    }

    public enum IEngineStatus
    {
        Ok,
        Running,
        Failed,
        OutOfDate
    }
    
}