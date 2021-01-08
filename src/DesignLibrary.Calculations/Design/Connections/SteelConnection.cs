using System.Collections.Generic;
using TLS.DesignLibrary.Calculations.Analysis;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Calculations.Design.Connections
{
    public abstract class SteelConnection : ContextualCalculation
    {
        public double AngleOffHorizontal { get; set; }
        public double Bearing { get; set; }


        [Input("SteelConnection_MajorShearForce_Name", "SteelConnection_MajorShearForce_Description", "SteelConnection_MajorShearForce_Group")]
        public List<double> MajorShearForce { get; set; }
        [Input("SteelConnection_MinorShearForce_Name", "SteelConnection_MinorShearForce_Description", "SteelConnection_MinorShearForce_Group")]
        public List<double> MinorShearForce { get; set; }
        public List<double> MajorMoment { get; set; }
        public List<double> MinorMoment { get; set; }
        public List<double> AxialForce { get; set; }
        [Input("SteelConnection_TyingForce_Name", "SteelConnection_TyingForce_Description", "SteelConnection_TyingForce_Group")]
        public double TyingForce { get; set; }

        List<double> localMajorShearForce { get; set; }
        List<double> localMinorShearForce { get; set; }
        List<double> localMajorMoment { get; set; }
        List<double> localMinorMoment { get; set; }
        List<double> localAxialForce { get; set; }
        double localTyingForce { get; set; }
        double localTyingForceMajorShear { get; set; }
        double localTyingForceMinorShear { get; set; }

        public List<double> MajorShearUsage { get; private set; }
        public List<double> MinorShearUsage { get; private set; }
        public List<double> OverallShearUsage { get; private set; }
        public List<double> MajorMomentUsage { get; private set; }
        public List<double> MinorMomentUsage { get; private set; }
        [Output("SteelConnection_TyingUsage_Name", "SteelConnection_TyingUsage_Description", "SteelConnection_TyingUsage_Group")]
        public double TyingUsage { get; protected set; }

        public SteelConnection() : base()
        {
            MajorShearForce = new List<double>() { 0 };
            MinorShearForce = new List<double>() { 0 };

            MajorShearUsage = new List<double>() { 0 };
            MinorShearUsage = new List<double>() { 0 };
            MajorMomentUsage = new List<double>() { 0 };
            MinorMomentUsage = new List<double>() { 0 };
            OverallShearUsage = new List<double>() { 0 };
        }

        public override void ContextualRunInit(CalculationContext context)
        {
            MajorShearUsage = new List<double>(context.Combinations.Count);
            MinorShearUsage = new List<double>(context.Combinations.Count);
            MajorMomentUsage = new List<double>(context.Combinations.Count);
            MinorMomentUsage = new List<double>(context.Combinations.Count);
            OverallShearUsage = new List<double>(context.Combinations.Count);
            
            // Calculate tying resistance

            CheckDetailRequirements();
            CheckTying();

            base.ContextualRunInit(context);
        }

        public override void RunCombination(int combinationIndex, Combination combination, CalculationContext context)
        {
            LocalGlobalLoadConverter converter = new LocalGlobalLoadConverter();
            converter.Bearing = Bearing;
            converter.LocalAxial = AxialForce[combinationIndex];
            converter.LocalMajorShear = MajorShearForce[combinationIndex];
            converter.LocalMinorShear = MinorShearForce[combinationIndex];

            converter.Run(context.Output);

            localAxialForce[combinationIndex] = converter.GlobalAxial;
            localMajorShearForce[combinationIndex] = converter.GlobalMajorShear;
            localMinorShearForce[combinationIndex] = converter.GlobalMinorShear;
        }

        public abstract bool CheckDetailRequirements();

        public abstract void CheckTying();
    }
}
