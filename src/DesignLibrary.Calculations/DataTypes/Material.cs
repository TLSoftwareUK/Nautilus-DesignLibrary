namespace TLS.DesignLibrary.Calculations.DataTypes
{
    /// <summary>
    /// Class representing the physical properties of a material
    /// </summary>
    public class Material
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public double YoungsModulus { get; set; }
        public double YieldStrength { get; set; }
    }
}
