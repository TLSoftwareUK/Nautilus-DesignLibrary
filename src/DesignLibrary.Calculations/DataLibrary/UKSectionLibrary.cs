using System.Collections.Generic;
using System.Linq;
using Jpp.DesignCalculations.Calculations.DataTypes;
using Jpp.DesignCalculations.Calculations.Properties;

namespace Jpp.DesignCalculations.Calculations.DataLibrary
{
    public class UKSectionLibrary
    {
        public Dictionary<string, ICrossSection> UkbSections { get; private set; }

        public UKSectionLibrary()
        {
            UkbSections = new Dictionary<string, ICrossSection>();

            string data = Resources.UKB.Trim();
            string[] lines = data.Split('\n');
            List<ICrossSection> temp = new List<ICrossSection>();
            for (int i = 7; i < 103; i++)
            {
                string[] parts = lines[i].Split(',');
                ICrossSection section = new ICrossSection()
                {
                    Name = parts[0],
                    Area = double.Parse(parts[28].TrimEnd('\r')) / 10000,
                    Height = double.Parse(parts[3]) / 1000,
                    MajorSecondMomentOfArea = double.Parse(parts[16]) / 100000000,
                    WebThickness =  double.Parse(parts[5]) / 1000,
                    FlangeThickness = double.Parse(parts[6]) / 1000,
                };
                temp.Add(section);
            }

            temp.Reverse();

            foreach (ICrossSection section in temp)
            {
                UkbSections.Add(section.Name, section);
            }
        }
    }
}
