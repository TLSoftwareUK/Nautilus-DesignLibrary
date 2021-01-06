using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jpp.DesignCalculations.Calculations;
using Jpp.DesignCalculations.Engine.Project;

namespace Jpp.DesignCalculations.Engine
{
    public class CpuEngine : AbstractEngine
    {
        public override async Task RunAsync(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                await base.RunAsync(token).ConfigureAwait(false);

                while (!token.IsCancellationRequested && _workQueue.Any())
                {
                    Calculation c;
                    if(!_workQueue.TryDequeue(out c))
                        break;

                    switch (c)
                    {
                        case ContextualCalculation contextualCalculation:
                            //TODO: Rune contextual calcs
                            break;

                        case ContextlessCalculation contextlessCalculation:
                            contextlessCalculation.Run(Output);
                            break;

                    }
                }
            }).ConfigureAwait(false);

            Status = IEngineStatus.Ok;
        }
    }
}
