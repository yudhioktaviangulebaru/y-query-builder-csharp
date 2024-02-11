using BelajarSharp.database.QueryBuilder;

namespace QueryBuilderSharp.example.application_parameters
{

    public interface AppParamRepo
    {
        public Entity findById(long id);
        public List<Entity> list(int limit, int offset);
        public Entity save(Entity entity);
        public Entity update(long id, Entity entity);
    }
    public class Entity:Model
    {
        public long? Id { get; set; } 
        public String Name { get; set; } 
        public String Code { get; set; }
        public string Value { get; set; }
        public string GroupType { get; set; }
        
       public Entity():base()
        {
            this.SetTableModel("application_parameters","Name,Code,Value,GroupType");
        }
    }
    public class ApplicationParameterRepository:AppParamRepo
    {
        public Entity findById(long id)
        {
            return new Entity().findById<Entity>(id);
        }

        public List<Entity> list(int limit,int offset) {
            return new Entity().findAll<Entity>(limit, offset);
        }

        public Entity save(Entity entity) {
            return new Entity().save<Entity>(entity);
        }

        public Entity update(long id, Entity entity)
        {
            return new Entity().update<Entity>(id, entity);
        }

    }
}
