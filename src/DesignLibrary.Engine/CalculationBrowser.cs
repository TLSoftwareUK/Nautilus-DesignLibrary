using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TLS.DesignLibrary.Calculations;
using TLS.DesignLibrary.Calculations.Attributes;

namespace TLS.DesignLibrary.Engine
{
    public class CalculationBrowser
    {
        public IReadOnlyList<Calculation> AvailableCalculations
        {
            get
            {
                return _availableCalculations;
            }
        }

        private List<Calculation> _availableCalculations;

        public BrowserInfo Root
        {
            get { return _root; }
        }

        private BrowserInfo _root;

        public CalculationBrowser()
        {
            _availableCalculations = new List<Calculation>();
            _root = new BrowserInfo("Calculations");
            
            foreach(Assembly assems in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assems.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Calculation))))
                {
                    if(!type.IsDefined(typeof(HiddenCalculationAttribute), false))
                    {
                        _availableCalculations.Add((Calculation) Activator.CreateInstance(type));
                    }
                }
            }

            BuildTree();
        }

        private void BuildTree()
        {
            foreach (Calculation availableCalculation in AvailableCalculations)
            {
                string calcNamespace = availableCalculation.GetType().Namespace;
                calcNamespace = calcNamespace.Replace("Jpp.DesignCalculations.Calculations.", "");
                string[] parts = calcNamespace.Split('.');

                Queue<string> path = new Queue<string>(parts);
                RecursiveAdd(path, _root, availableCalculation);
            }
        }

        private void RecursiveAdd(Queue<string> path, BrowserInfo parent, Calculation calc)
        {
            if (path.Count >= 1)
            {
                string childId = path.Dequeue();
                BrowserInfo child = parent.Children.FirstOrDefault(bi => bi.Name == childId);
                if (child == null)
                {
                    child = new BrowserInfo(childId);
                    parent._children.Add(child);
                }

                RecursiveAdd(path, child, calc);
            }
            else
            {
                CalculationInfo info = new CalculationInfo(calc);
                parent._children.Add(info);
            }
        }
    }
}
