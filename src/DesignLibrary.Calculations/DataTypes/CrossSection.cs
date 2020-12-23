namespace Jpp.DesignCalculations.Calculations.DataTypes
{
    /// <summary>
    /// Class representing a structural cross section
    /// </summary>
    public class CrossSection
    {
        public string Name { get; set; }

        public double MajorPlasticSectionModulus { get; set; }
        public double MajorElasticSectionModulus { get; set; }

        public double MinorPlasticSectionModulus { get; set; }
        public double MinorElasticSectionModulus { get; set; }

        public double MajorSecondMomentOfArea { get; set; }
        public double MinorSecondMomentOfArea { get; set; }

        public double Area { get; set; }

        public double Height { get; set; }

        public virtual void CopyFrom(CrossSection selectedSection)
        {
            Name = selectedSection.Name;
            Area = selectedSection.Area;
            MajorElasticSectionModulus = selectedSection.MajorElasticSectionModulus;
            MajorPlasticSectionModulus = selectedSection.MajorPlasticSectionModulus;
            MinorElasticSectionModulus = selectedSection.MinorElasticSectionModulus;
            MinorPlasticSectionModulus = selectedSection.MinorPlasticSectionModulus;
            MajorSecondMomentOfArea = selectedSection.MajorSecondMomentOfArea;
            MinorSecondMomentOfArea = selectedSection.MinorSecondMomentOfArea;
            Height = selectedSection.Height;
        }
    }
}
