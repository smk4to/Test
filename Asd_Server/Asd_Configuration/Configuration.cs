using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Asd_Configuration
{
    public static class Configuration
    {
        private static Asd_Configuration_Model Get()
        {
            try
            {
                var path = Path.Combine(AppContext.BaseDirectory, "Configuration.json");
                var json = File.ReadAllText(path, Encoding.UTF8);
                var obj = JsonConvert.DeserializeObject<Asd_Configuration_Model>(json);
                return obj;
            }
            catch
            {
                return null;
            }
        }

        private static Asd_Configuration_Database_Model Database(Asd_E_Database_Type type)
        {
            return type switch
            {
                Asd_E_Database_Type.Local => Get()?.LocalDatabase,
                _ => null
            };
        }

        public static Asd_E_Database_System? LocalDatabaseSystem()
        {
            var database = Database(Asd_E_Database_Type.Local);
            return database?.System;
        }

        public static string LocalConnectionString()
        {
            var database = Database(Asd_E_Database_Type.Local);
            return database == null
                ? null
                : database.System switch
                {
                    Asd_E_Database_System.Postgres => ("host=" + database.Server + "; database=" + database.Database + "; user id=" + database.User + ";" + " password=" + database.Password),
                    Asd_E_Database_System.MsSql => (database.Server.StartsWith("(localdb)")
                        ? "Server=" + database.Server + "; Database=" + database.Database + "; Trusted_Connection=True; MultipleActiveResultSets=true"
                        : "Initial Catalog=" + database.Database + "; User Id=" + database.User + "; Password=" + database.Password + "; Connect Timeout=60"),
                    _ => null
                };
        }
    }
}
