using System;
using System.Collections.Generic;
using Jpp.DesignCalculations.Calculations.Analysis.Bars;
using Jpp.DesignCalculations.Calculations.Attributes;
using Jpp.DesignCalculations.Calculations.DataTypes;
using MathNet.Numerics.LinearAlgebra;

namespace Jpp.DesignCalculations.Calculations.Analysis.Solvers
{
    [HiddenCalculation]
    public partial class TrussSolver2d : GenericSolver
    {
        private const int DEGREES_OF_FREEDOM = 2;

        public TrussSolver2d() : base()
        {
        }
        
        public override void ContextualRunInit(CalculationContext context)
        {
            IEnumerable<Point3d> joints = BuildJointList();
            Matrix<double> jointStructure = BuildJointStructure(joints);
            Matrix<double> supportStructure = BuildSupportStructure(joints);
            Matrix<double> memberStructure = BuildMemberStructure(joints);
            Vector<double> coordinateStructure = BuildCoordinateNumberStructure(joints, supportStructure);
            (Vector<double> loadJoints, Matrix<double> loadMagnitude) = BuildLoadStructures(joints);
            Vector<double> loadStructure = BuildLoadVector(joints, loadJoints, loadMagnitude, coordinateStructure); //Condense with previous line?
            Matrix<double> stiffnessMatrixStructure = BuildStiffnessMatrix(joints, memberStructure, jointStructure, coordinateStructure);
            Vector<double> displacements = stiffnessMatrixStructure.Solve(loadStructure);

            throw new NotImplementedException();
        }

        public override void RunCombination(int combinationIndex, Combination combination, CalculationContext context)
        {
            throw new NotImplementedException();
        }
    }

    public struct JointDisplacement
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}

