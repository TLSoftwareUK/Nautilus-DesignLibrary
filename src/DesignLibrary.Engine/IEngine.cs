using System.Threading;
using System.Threading.Tasks;
using TLS.DesignLibrary.Calculations;
using TLS.DesignLibrary.Calculations.Output;

namespace TLS.DesignLibrary.Engine
{
    public interface IEngine
    {
        OutputBuilder Output { get; }
        
        IEngineStatus Status { get; }

        void SetContainer(CalculationContainer container);

        Task RunAsync(CancellationToken token);

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
