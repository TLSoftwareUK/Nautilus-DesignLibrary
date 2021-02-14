using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TLS.DesignLibrary.Calculations;

namespace TLS.DesignLibrary.Engine
{
    public class CpuEngine : AbstractEngine
    {
        public override async Task RunAsync(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                await base.RunAsync(token).ConfigureAwait(false);
                
                Output.BeginOutput();
                
                //Build context
                //TODO: Remove to list, temp to get working
                CalculationContext cc = new CalculationContext(_container.LoadCases.ToList(), _container.Combinations.ToList(), Output);

                while (!token.IsCancellationRequested && _workQueue.Any())
                {
                    Calculation c;
                    if(!_workQueue.TryDequeue(out c))
                        break;

                    switch (c)
                    {
                        case ContextualCalculation contextualCalculation:
                            contextualCalculation.Run(cc);
                            break;

                        case ContextlessCalculation contextlessCalculation:
                            contextlessCalculation.Run(Output);
                            break;

                    }
                }
            }).ConfigureAwait(false);

            _container.Output = Output.EndOutput();
            
            Status = IEngineStatus.Ok;
        }
    }
}
