namespace TodoManagerApp.DAL.Data
{
    public class Todo
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Deadline { get; set; }
        public int Priority { get; set; }
        public int ColumnID { get; set; }

        public Column Column { get; set; }
    }
}
