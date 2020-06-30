namespace Asd_License
{
    public class Asd_License_Model
    {
        public string Hash { get; set; }
        public Asd_Hardware.Asd_Hardware_Model Hardware { get; set; }
        public Asd_License_Info_Model Info { get; set; }
        public Asd_License_Person_Model Person { get; set; }
    }

    public class Asd_License_Info_Model
    {
        public int Kiosks { get; set; }
        public int Workplaces { get; set; }
        public string Expires { get; set; }
    }

    public class Asd_License_Person_Model
    {
        public string Company { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
