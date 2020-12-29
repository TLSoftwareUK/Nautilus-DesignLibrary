using System.Threading.Tasks;
using Jpp.DesignCalculations.Calculations;
using Jpp.DesignCalculations.Engine.Project;

namespace Jpp.DesignCalculations.Engine
{
    public class CpuEngine : AbstractEngine
    {
        public override async Task RunAsync()
        {
            Output.Clear();
            await base.RunAsync();

            RunContextlessCalcs();
        }

        private void RunContextlessCalcs()
        {
            foreach (EngineCalculation calculation in _container.ContextlessCalculations)
            {
                ContextlessCalculation calc = (ContextlessCalculation) calculation.Calc;
                calc.Run(Output);
            }
        }
    }
}
