using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelTodo = TodoManagerApp.DAL.Models.Todo;

namespace TodoManagerApp.DAL.Models
{
    public interface ITodoRepository
    {
        Task<ModelTodo> GetTodoOrNull(int todoID);
        Task<IReadOnlyCollection<ModelTodo>> TodoList(int columnID = -1);
        Task<IReadOnlyCollection<ModelTodo>> ReversedPriorityTodoList(int columnID = -1);
        Task<int> InsertTodo(ModelTodo value);
        Task<string> DeleteTodo(int todoId);
        Task<string> UpdateTodo(ModelTodo value);
    }
}
