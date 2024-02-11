namespace BelajarSharp.database.migrationcore
{

    public interface BluePrint {
        public void increments();
        public void text(string name,int length = 200,bool unique=false);
        public void number(string name,int length =11);
        public void timestamps();
        public void generate();
        public String getTableSQL();
    }
    public class BlueprintImpl : BluePrint
    {

        private List<String> bluePrintTable;
        private List<String> additionalParams;
        private String tableSQL = "(@tableSQL, @AdditionalParam)";
        
        private BlueprintImpl() {
            bluePrintTable = new List<string>();
            additionalParams = new List<string>();
        }

        public void generate()
        {
            String join = String.Join(",", bluePrintTable.ToArray());
            String additionalAttributes = String.Join(",", additionalParams.ToArray());
            tableSQL = tableSQL.Replace("@tableSQL", join).Replace("@AdditionalParam",additionalAttributes);
        }

        public String getTableSQL()
        {
            generate();
            return tableSQL;
        }

        public void increments()
        {
            String sql = "Id BIGINT(20) NOT NULL AUTO_INCREMENT";
            bluePrintTable.Add(sql);
            sql = "PRIMARY KEY(Id)";
            additionalParams.Add(sql);
        }

        public void number(string name, int length = 11)
        {
            String sql = "@name @tipedata(@size)";
            sql = sql.Replace("@name", name);
            sql = sql.Replace("@tipedata", "INT");
            sql = sql.Replace("@size", length + "");
            bluePrintTable.Add(sql);
        }

        public void text(string name, int length = 200,bool isUnique=false)
        {
            String sql = "@name @tipedata(@size)";
            sql = sql.Replace("@name", name);
            sql = sql.Replace("@tipedata", "VARCHAR");
            sql = sql.Replace("@size", length + "");
            if (isUnique) {
                additionalParams.Add("UNIQUE(" + name + ")");
            }
            bluePrintTable.Add(sql);
        }

        public void timestamps()
        {
            String sql = "CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ";
            bluePrintTable.Add(sql);
            sql = "UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP";
            bluePrintTable.Add(sql);
        }



        private static BlueprintImpl instance=null;
       
        public static BlueprintImpl builder() {
            if (instance == null)
            {
                instance = new BlueprintImpl();
            }
            return instance;
        }

        
    }
    

}
