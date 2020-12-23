using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.DesignCalculations.Engine.Project
{
    public abstract class EurocodeStandard : IProjectStandard
    {
        public abstract string Name { get; }
    }
}
