using BelajarSharp.constants;
using BelajarSharp.database.migrationcore;
using MySqlConnector;
using System.Diagnostics;

namespace BelajarSharp.configapp
{

    public class AutoMigration
    {
        private static List<string> commands;
        public static void runMigration()
        {
            commands = new List<string>();
            bool isMigrate = DB.getMigration();
            if (isMigrate)
            {
                run();
            }
            else
            {
                Trace.WriteLine("RUN WITHOUT MIGRATION");
            }
        }

        public static async void run()
        {
            var connectionString = DB.getConnectionString();
            using var connction = new MySqlConnection(connectionString);
            await connction.OpenAsync();
            Trace.WriteLine("STARTING MIGRATION");
            AppParamMigrate();
            using var sqlComm = new MySqlCommand("",connction);
            sqlComm.CommandType = System.Data.CommandType.Text;

            foreach (String command in commands)
            {
                try
                {
                    sqlComm.CommandText = command;
                    using var result = await sqlComm.ExecuteReaderAsync();
                    await result.CloseAsync();
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                    Trace.WriteLine(e.StackTrace);
                }

            }




            await connction.CloseAsync();
        }

        public static void AppParamMigrate()
        {
            Trace.WriteLine("AppParamMigrate START");
            BluePrint bluePrint =BlueprintImpl.builder();
            bluePrint.increments();
            bluePrint.text("Code",200,true);
            bluePrint.text("Name");
            bluePrint.text("Value");
            bluePrint.text("GroupType");
            var sql = Schema.createBluePrint("application_parameters", bluePrint);
            commands.Add(sql);
            Trace.WriteLine("AppParamMigrate STOP");
        }

        
    }
}
