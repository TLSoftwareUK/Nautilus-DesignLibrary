namespace Jpp.DesignCalculations.Calculations
{
    /// <summary>
    /// Base class for calculations that do not require a context to run with
    /// </summary>
    public abstract class ContextlessCalculation : Calculation
    {
        /// <summary>
        /// Call to run calculation and set outputs
        /// </summary>
        public void Run(OutputBuilder builder)
        {
            RunBegin(builder);
            RunBody(builder);
            RunEnd(builder);
        }

        protected abstract void RunBody(OutputBuilder builder);
    }
}
