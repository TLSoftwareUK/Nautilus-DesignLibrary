using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.DesignCalculations.Engine
{
    public class BrowserInfo
    {
        public IReadOnlyList<BrowserInfo> Children
        {
            get { return _children; }
        }

        internal List<BrowserInfo> _children;

        public string Name { get; private set; }

        public BrowserInfo(string name)
        {
            Name = name;
            _children = new List<BrowserInfo>();
        }

    }
}
