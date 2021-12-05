using System.Collections.Generic;

namespace TodoManagerApp.DAL.Data
{
    public class Column
    {
        public Column()
        {
            Todos = new HashSet<Todo>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public ICollection<Todo> Todos { get; set; }
    }
}
