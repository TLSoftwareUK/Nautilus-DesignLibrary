using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jpp.DesignCalculations.Calculations.Analysis.Bars;
using Jpp.DesignCalculations.Calculations.DataTypes;
using MathNet.Numerics.LinearAlgebra;

namespace Jpp.DesignCalculations.Calculations.Analysis.Solvers
{
    public abstract class GenericSolver : ContextualCalculation
    {
        public List<AbstractBar> Bars { get; private set; }
        public List<Support> Supports { get; private set; }
        public List<PointLoad> Loads { get; private set; }

        public GenericSolver()
        {
            Bars = new List<AbstractBar>();
            Supports = new List<Support>();
            Loads = new List<PointLoad>();
        }

        internal IEnumerable<Point3d> BuildJointList()
        {
            List<Point3d> joints = new List<Point3d>();
            foreach (AbstractBar bar in Bars)
            {
                if (!joints.Contains(bar.StartPoint))
                    joints.Add(bar.StartPoint);

                if (!joints.Contains(bar.EndPoint))
                    joints.Add(bar.EndPoint);
            }

            return joints;
        }

        internal Matrix<double> BuildJointStructure(IEnumerable<Point3d> joints)
        {
            Matrix<double> jointStructure = Matrix<double>.Build.Dense(joints.Count(), 2);

            for (int i = 0; i < joints.Count(); i++)
            {
                Point3d point = joints.ElementAt(i);

                //Review for DoF
                jointStructure[i, 0] = point.X;
                jointStructure[i, 1] = point.Y;
            }

            return jointStructure;
        }
    }
}
