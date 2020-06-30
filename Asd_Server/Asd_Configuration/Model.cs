namespace Asd_Configuration
{
    public enum Asd_E_Database_Type { Local, Mainframe }

    public class Asd_Configuration_Model
    {
        public Asd_Configuration_Database_Model LocalDatabase { get; set; }
    }

    public class Asd_Configuration_Database_Model
    {
        public Asd_E_Database_System System { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

    public enum Asd_E_Database_System { Postgres, MsSql }
}
