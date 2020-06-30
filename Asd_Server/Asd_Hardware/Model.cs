namespace Asd_Hardware
{
    public class Asd_Hardware_Model
    {
        public string OperatingSystem { get; set; }
        public Asd_Cpu_Model Cpu { get; set; }
        public Asd_SmBios_Model SmBios { get; set; }
    }

    public class Asd_SmBios_Model
    {
        public string BiosVersion { get; set; }
        public string BiosVendor { get; set; }
        public string BiosCodename { get; set; }
        public string BoardVendor { get; set; }
        public string BoardName { get; set; }
        public string BoardVersion { get; set; }
    }

    public class Asd_Cpu_Model
    {
        public uint PhysicalCores { get; set; }
        public uint LogicalCores { get; set; }
        public string Architecture { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public uint Model { get; set; }
        public uint Family { get; set; }
    }
}
