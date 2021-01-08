using System;
using System.Collections.Generic;
using TLS.DesignLibrary.Calculations.Attributes;
using TLS.DesignLibrary.Calculations.DataTypes;
using TLS.DesignLibrary.Calculations.DataTypes.Connections;
using TLS.DesignLibrary.Calculations.Design.Connections.Parts;
using TLS.DesignLibrary.Calculations.Properties;

namespace TLS.DesignLibrary.Calculations.Design.Connections
{
    // TODO: Verify incoming resistance at notch
    class EndPlateConnection : SteelConnection
    {
        public Plate IncomingEndPlate { get; set; }

        [Input("EndPlateConnection_BoltRows_Name", "EndPlateConnection_BoltRows_Description", "EndPlateConnection_BoltRows_Group")]
        public List<BoltRow> BoltRows { get; private set; }

        [Input("EndPlateConnection_SupportingMemberSection_Name", "EndPlateConnection_SupportingMemberSection_Description", "EndPlateConnection_SupportingMemberSection_Group")]
        public CrossSection SupportingMemberSection { get; set; }
        [Input("EndPlateConnection_SupportingMemberRotation_Name", "EndPlateConnection_SupportingMemberRotation_Description", "EndPlateConnection_SupportingMemberRotation_Group")]
        public ColumnRotation SupportingMemberRotation { get; set; }
        [Input("EndPlateConnection_IncomingMemberSection_Name", "EndPlateConnection_IncomingMemberSection_Description", "EndPlateConnection_IncomingMemberSection_Group")]
        public CrossSection IncomingMemberSection { get; set; }

        private SupportingColumn _supportingColumn;

        public EndPlateConnection() : base()
        {
            CalculationName = Resources.EndPlateConnection_CalculationName;
            Description = Resources.EndPlateConnection_Description;
            Code = Resources.EndPlateConnection_Code;
            BoltRows = new List<BoltRow>();

            _supportingColumn = new SupportingColumn();
        }

        public override void ContextualRunInit(CalculationContext context)
        {
            base.ContextualRunInit(context);
            foreach (BoltRow boltRow in BoltRows)
            {
                boltRow.Run(context.Output);
            }

            //Set up supporting member
            _supportingColumn.Run(context);
        }

        public override void RunCombination(int combinationIndex, Combination combination, CalculationContext context)
        {
            base.RunCombination(combinationIndex, combination, context);
            ShearChecks(combinationIndex, combination);
        }

        /// <summary>
        /// Verify detailing requirements against Check 1 of SCI P358
        /// </summary>
        /// <returns></returns>
        public override bool CheckDetailRequirements()
        {
            //Verify plate dimensions
            if (IncomingMemberSection.Height * 0.6 > IncomingEndPlate.MajorDimension)
            {
                return false;
            }

            if (IncomingEndPlate.Thickness < 10 || IncomingEndPlate.Thickness > 12)
            {
                return false;
            }

            // TODO: Add bolt checks

            return true;
        }

        

        public override void CheckTying()
        {
            double tensionResistance = 0;
            foreach (BoltRow boltRow in BoltRows)
            {
                tensionResistance += boltRow.TensionResistance;
            }

            TyingUsage = TyingForce / tensionResistance;
        }

        private void ShearChecks(int combinationIndex, Combination combination)
        {
            double resultantShear = Math.Sqrt(Math.Pow(MajorShearForce[combinationIndex], 2) +
                                              Math.Pow(MinorShearForce[combinationIndex], 2));

            /*double shearUsage = resultantShear / BoltGroup.ShearResistance;
            double majorBearingUsage = Math.Max(MajorShearForce[combinationIndex] / BoltGroup.Member1MajorBearingResistance, MajorShearForce[combinationIndex] / BoltGroup.Member2MajorBearingResistance);
            double minorBearingUsage = Math.Max(MinorShearForce[combinationIndex] / BoltGroup.Member1MinorBearingResistance, MinorShearForce[combinationIndex] / BoltGroup.Member2MinorBearingResistance);
            
            
            MajorShearUsage[combinationIndex] = Math.Min(majorBearingUsage, MajorShearForce[combinationIndex] / BoltGroup.ShearResistance);
            MinorShearUsage[combinationIndex] = Math.Min(majorBearingUsage, MajorShearForce[combinationIndex] / BoltGroup.ShearResistance);

            OverallShearUsage[combinationIndex] = new double[]
            {
                shearUsage,
                minorBearingUsage,
                minorBearingUsage
            }.Max();*/
        }
    }
}
