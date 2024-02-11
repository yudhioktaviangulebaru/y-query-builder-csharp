using System.Linq.Expressions;

namespace BelajarSharp.constants
{
    public class DB
    {


        public static IConfiguration setCOnfig() { 
            return new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
        }
        public static string getConnectionString() {
            IConfiguration c = setCOnfig(); 
            return c.GetConnectionString("Default");
        }

        public static Boolean getMigration() {
            IConfiguration c = setCOnfig();
            var migration =  c.GetValue("Migrate",false);
            return migration;
        }
    }
}
