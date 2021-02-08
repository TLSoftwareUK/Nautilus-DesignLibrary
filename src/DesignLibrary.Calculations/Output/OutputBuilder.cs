using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TLS.DesignLibrary.Calculations.Output
{
    public class OutputBuilder
    {
        private XElement _outputTree;
        private Stack<XElement> _currentPath;

        public OutputBuilder(string existing = "")
        {
            _currentPath = new Stack<XElement>();

            if (!String.IsNullOrWhiteSpace(existing))
            {
                _outputTree = XElement.Parse(existing);
            }
            else
            {
                _outputTree = new XElement("output");
            }
        }
        
        public void BeginOutput()
        {
            _currentPath.Push(_outputTree);
        }

        public string EndOutput()
        {
            _currentPath.Clear();
            return _outputTree.ToString();
        }

        public void Clear()
        {

        }

        private string GetTemplate(Calculation calc)
        {
            string calcNamespace = calc.GetType().Namespace;
            calcNamespace = calcNamespace.Replace("TLS.DesignLibrary.Calculations.", "Output");
            calcNamespace = calcNamespace.Replace(".", "\\");
            string path = calcNamespace + ".xml";

            if (File.Exists(path))
            {
                //Process template
                return File.ReadAllText(path);
            }
            else
            {
                return $"<missing-template path=\"{path}\" />";
            }
        }

        public void BeginCalculation(Calculation calc)
        {
            //Find existing calc
            IEnumerable<XElement> foundElements =
                from el in _outputTree.Elements("calculation") where (string)el.Attribute("CalcId") == calc.Id.ToString() select el;

            XElement thisElement;
            
            if (foundElements.Any())
            {
                thisElement = foundElements.First();
            }
            else
            {
                XElement template = XElement.Parse(GetTemplate(calc));
                thisElement = new XElement("calculation", new XAttribute("CalcId", calc.Id), template);
                _currentPath.Peek().Add(thisElement);
                //Add to parent?
            }
            
            _currentPath.Push(thisElement);
        }

        public void EndCalculation()
        {
            _currentPath.Pop();
        }

        /*public void AddVariable(string key, string value)
        {
            if(_currentCalc.Count == 0)
                throw new InvalidOperationException("No active calculation to add to");

            _currentCalc.Peek().Variables.Add(key, value);
        }*/
    }
}
