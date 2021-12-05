namespace TodoManagerApp.DAL.Models
{
    public class Todo
    {
        public readonly int ID;
        public readonly string Title;
        public readonly string Description;
        public readonly string Deadline;
        public readonly int Priority;
        public readonly int ColumnID;

        public Todo (int id, string title, string description, string deadline, int priority, int columnID)
        {
            ID = id;
            Title = title;
            Description = description;
            Deadline = deadline;
            Priority = priority;
            ColumnID = columnID;
        }
    }
}
