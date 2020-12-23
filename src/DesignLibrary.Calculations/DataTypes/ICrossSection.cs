using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.DesignCalculations.Calculations.DataTypes
{
    public class ICrossSection : CrossSection
    {
        public double WebThickness { get; set; }
        public double FlangeThickness { get; set; }

        public void CopyFrom(ICrossSection selectedSection)
        {
            WebThickness = selectedSection.WebThickness;
            FlangeThickness = selectedSection.WebThickness;
            this.CopyFrom((CrossSection)selectedSection);
        }
    }
}
