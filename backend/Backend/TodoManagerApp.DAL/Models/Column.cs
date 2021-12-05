namespace TodoManagerApp.DAL.Models
{
    public class Column
    {
        public readonly int ID;
        public readonly string Name;
        public readonly int Priority;
        public Column(int id, string name, int priority)
        {
            ID = id;
            Name = name;
            Priority = priority;
        }
    }
}
