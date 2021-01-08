using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using TLS.DesignLibrary.Calculations.DataTypes;

namespace TLS.DesignLibrary.Calculations.Analysis.Solvers
{
    public partial class TrussSolver2d : GenericSolver
    {
        internal Matrix<double> BuildSupportStructure(IEnumerable<Point3d> joints)
        {
            // TODO : It is assumed one support per joint, correct this
            Matrix<double> supportStructure = Matrix<double>.Build.Dense(Supports.Count, DEGREES_OF_FREEDOM + 1);

            for (int i = 0; i < Supports.Count; i++)
            {
                int? jointNode = GetJoint(Supports[i].Location, joints);

                if (!jointNode.HasValue)
                    throw new InvalidOperationException("Support is not at a joint node");

                supportStructure[i, 0] = jointNode.Value;
                switch (Supports[i].Restraint)
                {
                    case JointRestraint.Free:
                        supportStructure[i, 1] = 0;
                        supportStructure[i, 2] = 0;
                        break;

                    case JointRestraint.RollerX:
                        supportStructure[i, 1] = 0;
                        supportStructure[i, 2] = 1;
                        break;

                    case JointRestraint.RollerY:
                        supportStructure[i, 1] = 1;
                        supportStructure[i, 2] = 0;
                        break;

                    case JointRestraint.Pinned:
                        supportStructure[i, 1] = 1;
                        supportStructure[i, 2] = 1;
                        break;

                    default:
                        throw new InvalidOperationException("Unkown restraint type");
                }
            }

            return supportStructure;
        }

        internal Matrix<double> BuildMemberStructure(IEnumerable<Point3d> joints)
        {
            Matrix<double> memberStructure = Matrix<double>.Build.Dense(Bars.Count, 4);
            //int? jointNode = GetJoint(Supports[i].Location, joints);

            for (int i = 0; i < Bars.Count; i++)
            {
                int? startId = GetJoint(Bars[i].StartPoint, joints);
                int? endId = GetJoint(Bars[i].EndPoint, joints);

                if (!startId.HasValue || !endId.HasValue)
                    throw new InvalidOperationException("Bar joints not recognized");

                memberStructure[i, 0] = startId.Value;
                memberStructure[i, 1] = endId.Value;
                memberStructure[i, 2] = Bars[i].Material.YoungsModulus;
                memberStructure[i, 3] = Bars[i].CrossSection.Area;
            }

            return memberStructure;
        }



        private int? GetJoint(Point3d joint, IEnumerable<Point3d> joints)
        {
            int? jointNode = null;
            for (int j = 0; j < joints.Count(); j++)
            {
                if (joint.Equals(joints.ElementAt(j)))
                {
                    jointNode = j;
                    break;
                }
            }

            return jointNode;
        }

        internal (Vector<double> loadJoints, Matrix<double> loadMagnitude) BuildLoadStructures(IEnumerable<Point3d> joints)
        {
            // TODO : It is assumed one load per joint, correct this
            Vector<double> loadJoints = Vector<double>.Build.Dense(Loads.Count);
            Matrix<double> loadMagnitude = Matrix<double>.Build.Dense(Loads.Count, 2);

            for (int i = 0; i < Loads.Count; i++)
            {
                PointLoad load = Loads.ElementAt(i);

                int? loadNode = GetJoint(load.Location, joints);
                if (!loadNode.HasValue)
                    throw new InvalidOperationException("Load is not at a joint node");

                loadJoints[i] = loadNode.Value;
                loadMagnitude[i, 0] = load.XForce;
                loadMagnitude[i, 1] = load.YForce;
            }

            return (loadJoints, loadMagnitude);
        }

        private int GetDegreesOfFreedom(int numberOfJoints)
        {
            int restraintCount = 0;
            foreach (Support support in Supports)
            {
                switch (support.Restraint)
                {
                    case JointRestraint.Free:
                        break;

                    case JointRestraint.RollerX:
                    case JointRestraint.RollerY:
                        restraintCount++;
                        break;

                    case JointRestraint.Pinned:
                        restraintCount = restraintCount + 2;
                        break;

                    default:
                        throw new InvalidOperationException("Unkown restraint type");
                }
            }
            return DEGREES_OF_FREEDOM * numberOfJoints - restraintCount;
        }

        internal Vector<double> BuildCoordinateNumberStructure(IEnumerable<Point3d> joints, Matrix<double> supportMatrix)
        {
            int j = 0;
            int k = GetDegreesOfFreedom(joints.Count());

            Vector<double> coordinateStructure = Vector<double>.Build.Dense(DEGREES_OF_FREEDOM * joints.Count());

            for (int i = 0; i < joints.Count(); i++)
            {
                //Point3d joint = joints.ElementAt(i);
                int iCount = 0;

                bool isSupport;
                int supportNumber;
                (isSupport, supportNumber) = IsJointSupport(i, supportMatrix);
                // TDOD: Refactor this to be neater and less convoluted
                if (isSupport)
                {
                    iCount = 1;
                    if (supportMatrix[supportNumber, 1] == 1)
                    {
                        coordinateStructure[i * 2] = k;
                        k++;
                    }
                    else
                    {
                        coordinateStructure[i * 2] = j;
                        j++;
                    }

                    if (supportMatrix[supportNumber, 2] == 1)
                    {
                        coordinateStructure[i * 2 + 1] = k;
                        k++;
                    }
                    else
                    {
                        coordinateStructure[i * 2 + 1] = j;
                        j++;
                    }

                }
                else
                {
                    coordinateStructure[i * 2] = j;
                    j++;
                    coordinateStructure[i * 2 + 1] = j;
                    j++;
                }
            }

            return coordinateStructure;
        }

        private (bool, int) IsJointSupport(int joint, Matrix<double> supportMatrix)
        {
            for (int i = 0; i < supportMatrix.RowCount; i++)
            {
                if (supportMatrix[i, 0] == joint)
                {
                    return (true, i);
                }
            }

            return (false, 0);
        }

        internal Matrix<double> BuildStiffnessMatrix(IEnumerable<Point3d> joints, Matrix<double> memberStructure, Matrix<double> jointStructure, Vector<double> coordinateStructure)
        {
            int DoF = GetDegreesOfFreedom(joints.Count());

            Matrix<double> stiffnessMatrix = Matrix<double>.Build.Dense(DoF, DoF);

            //Iterate over all members
            for (int i = 0; i < memberStructure.RowCount; i++)
            {
                int startJointNumber = (int)memberStructure[i, 0];
                int endJointNumber = (int)memberStructure[i, 1];
                double youngsModulus = memberStructure[i, 2];
                double crossSectionArea = memberStructure[i, 3];
                double xStart = jointStructure[startJointNumber, 0];
                double yStart = jointStructure[startJointNumber, 1];
                double xEnd = jointStructure[endJointNumber, 0];
                double yEnd = jointStructure[endJointNumber, 1];

                double length = Math.Sqrt(Math.Pow(xEnd - xStart, 2) + Math.Pow(yEnd - yStart, 2));
                double cX = (xEnd - xStart) / length;
                double cY = (yEnd - yStart) / length;

                Matrix<double> memberMatrix = BuildGlobalMemberStiffnessMatrix(youngsModulus, crossSectionArea, length, cX, cY);

                //Add member matrix to full matrix
                AddMemberMatrixToGlobal(coordinateStructure, startJointNumber, endJointNumber, DoF, stiffnessMatrix, memberMatrix);
            }

            return stiffnessMatrix;
        }

        private static void AddMemberMatrixToGlobal(Vector<double> coordinateStructure, int startJointNumber, int endJointNumber,
            int DoF, Matrix<double> stiffnessMatrix, Matrix<double> memberMatrix)
        {
            for (int row = 0; row < 2 * DEGREES_OF_FREEDOM; row++)
            {
                int x, y;
                int rowIndex, columnIndex;

                if (row < DEGREES_OF_FREEDOM)
                {
                    rowIndex = startJointNumber * DEGREES_OF_FREEDOM + row;
                }
                else
                {
                    rowIndex = endJointNumber * DEGREES_OF_FREEDOM + (row - DEGREES_OF_FREEDOM);
                }

                x = (int) coordinateStructure[rowIndex];

                if (x < DoF)
                {
                    for (int column = 0; column < 2 * DEGREES_OF_FREEDOM; column++)
                    {
                        if (column < DEGREES_OF_FREEDOM)
                        {
                            columnIndex = startJointNumber * DEGREES_OF_FREEDOM + column;
                        }
                        else
                        {
                            columnIndex = endJointNumber * DEGREES_OF_FREEDOM + (column - DEGREES_OF_FREEDOM);
                        }

                        y = (int) coordinateStructure[columnIndex];
                        if (y < DoF)
                        {
                            stiffnessMatrix[x, y] += memberMatrix[row, column];
                        }
                    }
                }
            }
        }

        private Matrix<double> BuildGlobalMemberStiffnessMatrix(double youngs, double crossSection, double length, double cX, double cY)
        {
            double z = youngs * crossSection / length;
            double z1 = z * Math.Pow(cX, 2);
            double z2 = z * Math.Pow(cY, 2);
            double z3 = z * cX * cY;

            Matrix<double> result = Matrix<double>.Build.DenseOfArray(new[,] {
                { z1, z3, -z1, -z3},
                { z3, z2, -z3, -z2},
                { -z1, -z3, z1, z3},
                { -z3, -z2, z3, z2}
            });

            return result;
        }

        internal Vector<double> BuildLoadVector(IEnumerable<Point3d> joints, Vector<double> loadJoints, Matrix<double> loadMagnitude, Vector<double> coordinateStructure)
        {
            int DoF = GetDegreesOfFreedom(joints.Count());

            Vector<double> result = Vector<double>.Build.Dense(DoF);
            for (int i = 0; i < loadJoints.Count; i++)
            {
                int rowIndex = (int)loadJoints[i];
                int rowIndex2 = (rowIndex) * DEGREES_OF_FREEDOM;

                for (int j = 0; j < DEGREES_OF_FREEDOM; j++)
                {
                    int tempLookup = rowIndex2 + j;
                    //rowIndex2 = rowIndex2 + 1; //Notsure this is warranted. Think it goes after the lookup
                    int n = (int)coordinateStructure[tempLookup];
                    if (n < DoF)
                    {
                        result[n] += loadMagnitude[i, j];
                    }
                }
            }

            return result;
        }

        internal IEnumerable<JointDisplacement> ExtractDisplacements(IEnumerable<Point3d> joints, Vector<double> displacements, Vector<double> coordinateStructure)
        {
            List<JointDisplacement> jointDisplacement = new List<JointDisplacement>();
            int DoF = GetDegreesOfFreedom(joints.Count());

            for (int i = 0; i < coordinateStructure.Count / 2; i++)
            {
                JointDisplacement joint = new JointDisplacement();
                int x = (int)coordinateStructure[i * 2];
                if (x < DoF)
                {
                    joint.X = displacements[x];
                }
                int y = (int)coordinateStructure[i * 2 + 1];
                if (y < DoF)
                {
                    joint.Y = displacements[y];
                }
                jointDisplacement.Add(joint);
            }

            return jointDisplacement;
        }
    }
}
