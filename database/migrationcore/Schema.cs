using System.Diagnostics;

namespace BelajarSharp.database.migrationcore
{
    public class Schema
    {
        public static string sql;
        public static String createBluePrint(string tableName,BluePrint bluePrint) {
            sql = ("CREATE TABLE IF NOT EXISTS @TABLE_NAME @BLUEPRINTS")
                .Replace("@TABLE_NAME", tableName)
                .Replace("@BLUEPRINTS",bluePrint.getTableSQL());

            Trace.WriteLine("SCHEMA GENERATOR For\n");
            Trace.WriteLine(tableName.ToUpper()+"\n");
            Trace.WriteLine("================ \n");
            return sql;

        }
    }
}
