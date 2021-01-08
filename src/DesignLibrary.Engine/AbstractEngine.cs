using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jpp.DesignCalculations.Calculations;
using Jpp.DesignCalculations.Engine.Project;

namespace Jpp.DesignCalculations.Engine
{
    public abstract class AbstractEngine : IEngine
    {
        protected CalculationContainer? _container;

        public OutputBuilder Output { get; private set; }
        public IEngineStatus Status { get; protected set; } = IEngineStatus.Ok;

        protected ConcurrentQueue<Calculation> _workQueue;
        
        public AbstractEngine()
        {
            Output = new OutputBuilder();

            _workQueue = new ConcurrentQueue<Calculation>();
        }

        public void SetContainer(CalculationContainer container)
        {
            _container = container;
            InvalidateAwaitingCalcs();
        }

        public virtual async Task RunAsync(CancellationToken token)
        {
            if (_container == null)
                throw new InvalidOperationException("Container required to run");

            Status = IEngineStatus.Running;
            Output.Clear();
        }

        public void InvalidateCalculation(Calculation calc)
        {
            if(!_workQueue.Contains(calc))
                _workQueue.Enqueue(calc);
            
            Status = IEngineStatus.OutOfDate;
        }

        private void InvalidateAwaitingCalcs()
        {
            foreach (EngineCalculation containerCalculation in _container.Calculations)
            {
                if (containerCalculation.Status == Project.Status.WaitingCalc)
                {
                    InvalidateCalculation(containerCalculation.Calc);
                }
            }
        }
    }
}
