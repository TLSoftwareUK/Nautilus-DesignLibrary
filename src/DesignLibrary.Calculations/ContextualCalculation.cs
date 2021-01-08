using System.Collections.Generic;
using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Calculations
{
    /// <summary>
    /// Base class for calculations requiring a context to run
    /// </summary>
    public abstract class ContextualCalculation : Calculation
    {
        public List<ContextlessCalculation> ContextlessSubCalculations { get; private set; } = new List<ContextlessCalculation>();
        
        /// <summary>
        /// Call to run calculation and set outputs
        /// </summary>
        public void Run(CalculationContext context)
        {
            RunBegin(context.Output);

            foreach (ContextlessCalculation contextlessSubCalculation in ContextlessSubCalculations)
            {
                contextlessSubCalculation.Run(context.Output);
            }
            
            ContextualRunInit(context);

            int i = 0;
            foreach (Combination contextCombination in context.Combinations)
            {
                RunCombination(i, contextCombination, context);
                i++;
            }

            RunEnd(context.Output);
        }

        public virtual void ContextualRunInit(CalculationContext context)
        {
        }

        public abstract void RunCombination(int combinationIndex, Combination combination, CalculationContext context);

        public virtual void SetContextualProperties(int combinationCount)
        { }

    }
}
