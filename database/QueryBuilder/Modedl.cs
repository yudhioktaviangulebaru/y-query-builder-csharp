using BelajarSharp.constants;
using MySqlConnector;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection.PortableExecutable;

namespace BelajarSharp.database.QueryBuilder
{
    
    public class Model
    {
        private string TableName = "";
        protected String fillable = "";
        private object lastId = 0;
        private dynamic resultOne;

        protected void SetTableModel(String name,String _fillable) {
            TableName = name;
            fillable = _fillable;
        }
        public IEnumerable<Dictionary<String, object>> executeQuery<T>(String sql,bool isList,bool isReturnTargetClass,Dictionary<String,object> additioinalParams=null) {
            var listData = new List<Dictionary<String, object>>();
            try {
                using var conn = new MySqlConnection(DB.getConnectionString());
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = sql;
                if (additioinalParams != null)
                {
                    var keys = additioinalParams.Keys.ToArray();
                    foreach(var key in keys)
                    {
                        var akey = "@" + key;
                        var aVal = additioinalParams[key];
                        command.Parameters.AddWithValue(akey, aVal);
                    }

                }

                
                if (isReturnTargetClass)
                {
                    var rs = command.ExecuteReader();
                    var cols = new List<string>();
                    for (var i = 0; i < rs.FieldCount; i++)
                        cols.Add(rs.GetName(i));

                    while (rs.Read())
                    {
                        var data = new Dictionary<String, object>();
                        foreach (var col in cols)
                            data.Add(col, rs[col]);
                        listData.Add(data);
                    }
                    
                    rs.Close();
                }
                else
                {
                     command.ExecuteNonQuery();
                    lastId = command.LastInsertedId;
                }
                 conn.Close();
                
            }
            catch (Exception e) { 
            }
            return listData;

        }

        public T findById<T>(long id)
        {
            var sqb = QueryBuilder.instance(this.TableName)
                .select("*")
                .where("Id","=",id).limit(1+"");
            var sql = sqb.Sql();
            var result = executeQuery<T>(sql, true, true,sqb.GetParams());
            object objData=new Object();
            foreach(var res in result)
            {
                var stringJSON = this.ToJSON(res);
                objData = JsonConvert.DeserializeObject<T>(stringJSON);
            
            }
            if (objData != null)
            {
                return (T)objData;
            }
            else {
                throw new Exception("DATA NOT FOUND");
            }
        }

        public List<T> findAll<T>(int limit = 10, int offset = 0)
        {
            var sql = QueryBuilder.instance(this.TableName)
                .select("*")
                .limit(limit+"")
                .offset(offset+"")
                .Sql();
            Trace.WriteLine(sql);
            var result = executeQuery<T>(sql, true, true);
            
            List<T> datas = new List<T>();
            foreach (var item in result)
            {
                var stringJSON = this.ToJSON(item);
                var objData = JsonConvert.DeserializeObject<T>(stringJSON);
                datas.Add(objData);

            }
            
            return datas;
        }

        public T save<T>(T data)
        {
            var createObject = QueryBuilder.instance(this.TableName)
                        .create<T>(data, fillable);

            var sqlParameter = createObject.GetSqlParams();
            var sql = createObject.Sql();
            executeQuery<T>(sql, false, false, sqlParameter);
            return findById<T>((long)lastId);
        }

        public T update<T>(long id,T data)
        {
            var updateObject = QueryBuilder.instance(this.TableName).Update<T>(id, data, this.fillable);
            var sqlParameter = updateObject.GetSqlParams();
            var sql = updateObject.Sql();
            executeQuery<T>(sql, false, false, sqlParameter);
            return findById<T>(id);
        }

        private String ToJSON(Dictionary<String,object> data)
        {
            return JsonConvert.SerializeObject(data);
        }
        private object DeserializeJSON(String json)
        {
            return JsonConvert.DeserializeObject(json);
        }
    }
}
