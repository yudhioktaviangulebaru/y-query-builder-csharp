namespace BelajarSharp.database.QueryBuilder
{
    public class QueryBuilder
    {
        private static QueryBuilder instances = null;
        private String table;
        private QueryBuilder() { }


        private QueryBuilder setTable(String _table) {
            table = _table;
            return this;
        }

        public static QueryBuilder instance(String tableName) {
            if (instances == null){
                instances = new QueryBuilder();
            }
            return instances.setTable(tableName);
        }

        public Select select(String field)
        {
           
            return new Select(table,field);
        }

        public Create create<ClassEntities>(ClassEntities e,String fillable)
        {
            return new Create(table,fillable).create<ClassEntities>(e);
        }
        public Create Update<ClassEntities>(long id, ClassEntities e,String fillable)
        {
            return new Create(table,fillable).Update<ClassEntities>(id,e);
        }

    }
}
