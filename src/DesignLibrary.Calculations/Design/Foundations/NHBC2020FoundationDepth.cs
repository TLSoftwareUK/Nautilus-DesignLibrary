using System;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.Output;
using TLS.DesignLibrary.Calculations.Properties;

namespace TLS.DesignLibrary.Calculations.Design.Foundations
{
    public class NHBC2020FoundationDepth : FoundationDepth
    {
        [Input("NHBC2020FoundationDepth_SoilPlasticity_Name", 
            "NHBC2020FoundationDepth_SoilPlasticity_Description", 
            "NHBC2020FoundationDepth_SoilPlasticity_Group")]
        public VolumeChangePotential? SoilPlasticity { get; set; }

        [Input("NHBC2020FoundationDepth_ExistingGroundLevel_Name",
            "NHBC2020FoundationDepth_ExistingGroundLevel_Description",
            "NHBC2020FoundationDepth_ExistingGroundLevel_Group", 
            true, UnitTypes.Length)]
        public double? ExistingGroundLevel { get; set; }

        [Input("NHBC2020FoundationDepth_ProposedGroundLevel_Name",
            "NHBC2020FoundationDepth_ProposedGroundLevel_Description",
            "NHBC2020FoundationDepth_ProposedGroundLevel_Group", units:UnitTypes.Length)]
        public double? ProposedGroundLevel { get; set; }

        [Input("NHBC2020FoundationDepth_ExistingTreeInfluence_Name",
            "NHBC2020FoundationDepth_ExistingTreeInfluence_Description",
            "NHBC2020FoundationDepth_ExistingTreeInfluence_Group", units:UnitTypes.Length)]
        public double? ExistingTreeInfluence { get; set; }

        [Input("NHBC2020FoundationDepth_ProposedTreeInfluence_Name",
            "NHBC2020FoundationDepth_ProposedTreeInfluence_Description",
            "NHBC2020FoundationDepth_ProposedTreeInfluence_Group", units:UnitTypes.Length)]
        public double? ProposedTreeInfluence { get; set; }

        [Input("NHBC2020FoundationDepth_RemovedTreeInfluence_Name",
            "NHBC2020FoundationDepth_RemovedTreeInfluence_Description",
            "NHBC2020FoundationDepth_RemovedTreeInfluence_Group", units:UnitTypes.Length)]
        public double? RemovedTreeInfluence { get; set; }

        [Input("NHBC2020FoundationDepth_TopOfConcreteLevel_Name",
            "NHBC2020FoundationDepth_TopOfConcreteLevel_Description",
            "NHBC2020FoundationDepth_TopOfConcreteLevel_Group", units:UnitTypes.Length)]
        public double? TopOfConcreteLevel { get; set; }

        [Output("NHBC2020FoundationDepth_FoundationDepth_Name",
            "NHBC2020FoundationDepth_FoundationDepth_Description",
            "NHBC2020FoundationDepth_FoundationDepth_Group", units:UnitTypes.Length)]
        public double? FoundationDepth { get; private set; }

        [Output("NHBC2020FoundationDepth_MinimumFoundationDepth_Name",
            "NHBC2020FoundationDepth_MinimumFoundationDepth_Description",
            "NHBC2020FoundationDepth_MinimumFoundationDepth_Group", units:UnitTypes.Length)]
        public double? MinimumFoundationDepth { get; private set; }

        [Output("NHBC2020FoundationDepth_ExistingPlantingFoundationDepth_Name",
            "NHBC2020FoundationDepth_ExistingPlantingFoundationDepth_Description",
            "NHBC2020FoundationDepth_ExistingPlantingFoundationDepth_Group", units:UnitTypes.Length)]
        public double? ExistingPlantingFoundationDepth { get; private set; }

        [Output("NHBC2020FoundationDepth_RemovedExistingPlantingFoundationDepth_Name",
            "NHBC2020FoundationDepth_RemovedExistingPlantingFoundationDepth_Description",
            "NHBC2020FoundationDepth_RemovedExistingPlantingFoundationDepth_Group", units:UnitTypes.Length)]
        public double? RemovedExistingPlantingFoundationDepth { get; private set; }

        [Output("NHBC2020FoundationDepth_ProposedPlantingFoundationDepth_Name",
            "NHBC2020FoundationDepth_ProposedPlantingFoundationDepth_Description",
            "NHBC2020FoundationDepth_ProposedPlantingFoundationDepth_Group", units:UnitTypes.Length)]
        public double? ProposedPlantingFoundationDepth { get; private set; }

        public NHBC2020FoundationDepth() : base()
        {
            CalculationName = Resources.NHBC2020FoundationDepth_CalculationName;
            Description = Resources.NHBC2020FoundationDepth_Description;
            Code = Resources.NHBC2020FoundationDepth_Code;
        }

        protected override void RunBody(OutputBuilder builder)
        {
            ResetCalculation();
            VerifyInputs();

            SetMinimumDepth(ExistingGroundLevel.Value);
            ApplyTreeInfluences(ExistingGroundLevel.Value);
            SetMinimumThickness();

            Calculated = true;
        }

        private void ApplyTreeInfluences(double existingGroundLevel)
        {
            // Depth due to proposed trees as Figure 3
            if (ProposedGroundLevel.HasValue && ProposedTreeInfluence.HasValue)
            {
                ProposedPlantingFoundationDepth = ProposedGroundLevel - ProposedTreeInfluence;
                FoundationDepth = Math.Min(FoundationDepth.Value, ProposedPlantingFoundationDepth.Value);
            }

            // Depth due to proposed trees as Figure 1
            if (ExistingTreeInfluence.HasValue)
            {
                if (ProposedGroundLevel.HasValue)
                {
                    ExistingPlantingFoundationDepth = Math.Min(existingGroundLevel - ExistingTreeInfluence.Value,
                        ProposedGroundLevel.Value - ExistingTreeInfluence.Value);
                }
                else
                {
                    ExistingPlantingFoundationDepth = existingGroundLevel - ExistingTreeInfluence.Value;
                }

                FoundationDepth = Math.Min(FoundationDepth.Value, ExistingPlantingFoundationDepth.Value);
            }

            //Depth due to removed trees;
            if (RemovedTreeInfluence.HasValue)
            {
                RemovedExistingPlantingFoundationDepth = existingGroundLevel - RemovedTreeInfluence.Value;
                FoundationDepth = Math.Min(FoundationDepth.Value, RemovedExistingPlantingFoundationDepth.Value);
            }
        }

        private void SetMinimumDepth(double existingGroundLevel)
        {
            double minimumDepth = 0;
            minimumDepth = GetMinimumDepth(minimumDepth);

            // Minimum depth
            if (ProposedGroundLevel.HasValue)
            {
                MinimumFoundationDepth = Math.Min(ProposedGroundLevel.Value - minimumDepth,
                    existingGroundLevel - minimumDepth);
            }
            else
            {
                MinimumFoundationDepth = existingGroundLevel - minimumDepth;
            }

            FoundationDepth = MinimumFoundationDepth.Value;
        }

        private double GetMinimumDepth(double minimumDepth)
        {
            if (SoilPlasticity.HasValue)
            {
                switch (SoilPlasticity.Value)
                {
                    case VolumeChangePotential.High:
                        minimumDepth = 1;
                        break;

                    case VolumeChangePotential.Medium:
                        minimumDepth = 0.9;
                        break;

                    case VolumeChangePotential.Low:
                        minimumDepth = 0.75;
                        break;
                }
            }

            return minimumDepth;
        }

        private void SetMinimumThickness()
        {
            if (TopOfConcreteLevel.HasValue)
            {
                double thickness = TopOfConcreteLevel.Value - FoundationDepth.Value;
                if (thickness < 0.15)
                {
                    double additional = 0.15 - thickness;
                    FoundationDepth = FoundationDepth - additional;
                }
            }
        }
    }
}
