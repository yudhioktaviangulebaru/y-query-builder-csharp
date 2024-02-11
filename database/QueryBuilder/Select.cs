using BelajarSharp.constants;
using MySqlConnector;

namespace BelajarSharp.database.QueryBuilder
{
    public class Select
    {
        private String tableName = "";
        
        private List<String> conditions;
        private List<String> whereFields;
        private String fields;
        private String offsets, limits;

        private Dictionary<String, object> valuesCondition;
        public Select(String table,String field) {
            tableName = table;
            conditions = new List<string>();
            fields = field;
            valuesCondition = new Dictionary<String, object>();
            whereFields = new List<string>();
        }

        public Select where(String field,String operators,object val)
        {
            var wheres = "@field @operator @@field".Replace("@field",field).Replace("@operator",operators);
            whereFields.Add(field);
            conditions.Add(wheres);
            valuesCondition.Add(field, val);
            return this;
        }

        public Dictionary<String, object> GetParams() {
            return valuesCondition;
        }

        public Select offset(String offset) {
            offsets = offset;

            return this;
        }

        public Select limit(String limit)
        {
            limits = limit;
            return this;
        }


        public String Sql()
        {
            var conditionString = conditions.Count>0 ? " WHERE "+String.Join(" AND ", conditions.ToArray())+" ":"";


            return "SELECT @a FROM @b @c @d @e".Replace("@a",fields)
                .Replace("@b",tableName)
                .Replace("@c",conditionString)
                .Replace("@e",offsets==""||offsets==null||offsets=="0"?" OFFSET 0":" OFFSET "+offsets)
                .Replace("@d", limits == "" ? " LIMIT 0" : " LIMIT " + limits);
        }



        


    }
}
