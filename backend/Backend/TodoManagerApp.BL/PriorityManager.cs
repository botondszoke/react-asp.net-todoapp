using System.Threading.Tasks;
using ModelTodo = TodoManagerApp.DAL.Models.Todo;
using ModelColumn = TodoManagerApp.DAL.Models.Column;
using TodoManagerApp.DAL.Models;

namespace TodoManagerApp.BL
{
    public static class PriorityManager
    {
        public async static Task<string> IncreasePriority(int column, int oldPriority, int newPriority, ITodoRepository repository)
        {
            var reversedTodoList = await repository.ReversedPriorityTodoList(column);
            foreach (ModelTodo t in reversedTodoList)
            {
                if (t.Priority >= newPriority && t.Priority < oldPriority)
                {
                    if (await repository.UpdateTodo(
                        new ModelTodo(t.ID, t.Title, t.Description, t.Deadline, t.Priority + 1, t.ColumnID))
                        != "Success")
                    {
                        return "Conflict";
                    }
                }
            }
            return "Success";
        }

        public async static Task<string> IncreasePriority(int oldPriority, int newPriority, IColumnRepository repository)
        {
            var reversedColumnList = await repository.ReversedPriorityColumnList();
            foreach (ModelColumn c in reversedColumnList)
            {
                if (c.Priority >= newPriority && c.Priority < oldPriority)
                {
                    if (await repository.UpdateColumn(
                        new ModelColumn(c.ID, c.Name, c.Priority + 1)) 
                        != "Success")
                    {
                        return "Conflict";
                    }
                }
            }
            return "Success";
        }

        public async static Task<string> ReducePriority(int column, int oldPriority, int newPriority, ITodoRepository repository)
        {
            var todoList = await repository.TodoList(column);
            foreach (ModelTodo t in todoList)
            {

                if ((t.Priority <= newPriority || newPriority == -1) && t.Priority > oldPriority)
                {
                    if (await repository.UpdateTodo(
                        new ModelTodo(t.ID, t.Title, t.Description, t.Deadline, t.Priority - 1, t.ColumnID))
                        != "Success")
                    {
                        return "Conflict";
                    }
                }
            }
            return "Success";
        }

        public async static Task<string> ReducePriority(int oldPriority, int newPriority, IColumnRepository repository)
        {
            var columnList = await repository.ColumnList();
            foreach (ModelColumn c in columnList)
            {
                if ((c.Priority <= newPriority || newPriority == -1) && c.Priority > oldPriority)
                {
                    if (await repository.UpdateColumn(
                        new ModelColumn(c.ID, c.Name, c.Priority -1))
                        != "Success")
                    {
                        return "Conflict";
                    }
                }
            }
            return "Success";
        }

        public async static Task<int> GetMaximumPriority(int column, ITodoRepository repository)
        {
            var todoList = await repository.TodoList(column);
            int maxPriority = -1;
            foreach (ModelTodo t in todoList)
            {
                if (maxPriority < t.Priority)
                    maxPriority = t.Priority;
            }
            return maxPriority;
        }

        public async static Task<int> GetMaximumPriority(IColumnRepository repository)
        {
            var columnList = await repository.ColumnList();
            int maxPriority = -1;
            foreach(ModelColumn c in columnList)
            {
                if (maxPriority < c.Priority)
                    maxPriority = c.Priority;
            }
            return maxPriority;
        }
    }
}
