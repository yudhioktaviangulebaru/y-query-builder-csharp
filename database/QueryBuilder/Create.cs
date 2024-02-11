using Newtonsoft.Json;

namespace BelajarSharp.database.QueryBuilder
{
    public class Create
    {
        private String tableName = "";
        private String fillable = "";
        private Dictionary<String, object> SQLParams;
        private List<String> fields;
        private List<String> paramDatas;
        private string sql = "";
        public Create(string tableName, string fillable)
        {
            this.tableName = tableName;
            this.fillable = fillable;
            fields = new List<string>();
            paramDatas = new List<string>();
        }


        private List<String> getFields()
        {
            var lists = new List<String>();
            var splitters = fillable.Split(",");
            foreach(var field in splitters)
            {
                lists.Add(field);
            }
            return lists;
        }
        private List<String> getParamData()
        {
            var lists = new List<String>();
            var splitters = fillable.Split(",");
            foreach(var field in splitters)
            {
                lists.Add("@"+field);
            }
            return lists;
        }
        public Create create<ClassEntities>(ClassEntities e)
        {
            fields = getFields();
            paramDatas = getParamData();

            var jsonObject = this.deserialzeEntity<ClassEntities>(e);
            var fieldJoin = String.Join(" , ",fields.ToArray());
            var paramJoin = String.Join(" , ",paramDatas.ToArray());
            SQLParams = new Dictionary<String, object>();
            foreach(var field in fields)
            {
                SQLParams.Add(field, jsonObject[field]);
            }
            sql = "INSERT INTO @table (@field) VALUES (@value)"
                .Replace("@table",tableName)
                .Replace("@field",fieldJoin)
                .Replace("@value",paramJoin);

            return this;
        }

        private Dictionary<String, object> deserialzeEntity<ClassEntities>(ClassEntities e)
        {
            String jsonData = JsonConvert.SerializeObject(e);
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<String, object>>(jsonData);
            return jsonObject;
        }

        public Dictionary<String, object> GetSqlParams() {
            return SQLParams;
        }

        public String Sql()
        {
            return sql;
        }

        private List<String> paramDataUpdateProcessing(List<String> fields) {
            List<String> setter = new List<String>();
            var setterDefault = "@field = @@param";
            foreach (String field in fields)
            {
                setter.Add(setterDefault.Replace("@field",field).Replace("@param",field));
            }

            return setter;
        }
        public Create Update<Entity>(long id,Entity updatedata)
        {
            SQLParams = new Dictionary<string, object>();
            fields = getFields();
            paramDatas = getParamData();
            var jsonObject = deserialzeEntity<Entity>(updatedata);  
            var paramUpdate = paramDataUpdateProcessing(fields);
            var paramUpdateString = String.Join(" , ", paramUpdate.ToArray());
            foreach (var field in fields)
                SQLParams.Add(field, jsonObject[field]);

            SQLParams.Add("Id", id );
            sql = "UPDATE @TABLE SET @SETTER WHERE @CONDITION"
                .Replace("@TABLE",tableName)
                .Replace("@SETTER",paramUpdateString)
                .Replace("@CONDITION","Id = @id");
            return this;
        }
    }
}
