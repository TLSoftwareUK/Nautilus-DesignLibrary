using System;
using System.Threading.Tasks;
using Jpp.DesignCalculations.Calculations;

namespace Jpp.DesignCalculations.Engine
{
    public abstract class AbstractEngine : IEngine
    {
        protected CalculationContainer? _container;

        public OutputBuilder Output { get; private set; }
        public IEngineStatus Status { get; private set; } = IEngineStatus.Ok;

        public AbstractEngine()
        {
            Output = new OutputBuilder();
        }

        public void SetContainer(CalculationContainer container)
        {
            _container = container;
        }

        public virtual async Task RunAsync()
        {
            if (_container == null)
                throw new InvalidOperationException("Container required to run");
        }

        public void InvalidateCalculation(Calculation calc)
        {
            Status = IEngineStatus.OutOfDate;
        }
    }
}
