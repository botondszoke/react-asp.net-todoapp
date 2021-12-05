using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using TodoManagerApp.DAL.Models;
using ModelTodo = TodoManagerApp.DAL.Models.Todo;

namespace TodoManagerApp.BL
{
    public class TodoManager
    {
        private readonly ITodoRepository todoRepository;

        public TodoManager(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        public async Task<ModelTodo> GetTodoOrNull(int todoID)
            => await todoRepository.GetTodoOrNull(todoID);

        public async Task<IReadOnlyCollection<ModelTodo>> TodoList()
            => await todoRepository.TodoList();

        public async Task<string> DeleteTodo(int todoID)
        {

            using (var tran = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.RepeatableRead },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var todo = await todoRepository.GetTodoOrNull(todoID);

                if (todo == null)
                    return "Not found";

                var todoPriority = todo.Priority;
                var column = todo.ColumnID;

                var result = await todoRepository.DeleteTodo(todoID);
                if (result != "Success")
                    return result;

                result = await PriorityManager.ReducePriority(column, todoPriority, -1, todoRepository);
                if (result != "Success")
                    return "Conflict";

                tran.Complete();
                return "Success";
            }
        }

        public async Task<int> InsertTodo(ModelTodo todo)
        {
            if (todo.Priority == await PriorityManager.GetMaximumPriority(todo.ColumnID, todoRepository) + 1)
                return await todoRepository.InsertTodo(todo);
            return -2;
        }

        public async Task<string> UpdateTodo(ModelTodo todo)
        {
            var oldTodo = await todoRepository.GetTodoOrNull(todo.ID);

            if (oldTodo != null && oldTodo.Priority == todo.Priority && oldTodo.ColumnID == todo.ColumnID)
                return await todoRepository.UpdateTodo(todo);

            else if (oldTodo == null)
                return "Not found";

            else if (oldTodo.ColumnID != todo.ColumnID && todo.Priority == await PriorityManager.GetMaximumPriority(todo.ColumnID, todoRepository) + 1) {
                var result = await todoRepository.UpdateTodo(todo);
                if (result != "Success")
                    return result;

                result = await PriorityManager.ReducePriority(oldTodo.ColumnID, oldTodo.Priority, -1, todoRepository);
                if (result != "Success")
                    return result;

                return "Success";
            }
            
            return "Bad request";
        }

        public async Task<string> ModifiedTodoPriority(ModelTodo todo)
        {
            using (var tran = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.RepeatableRead },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var oldTodo = await todoRepository.GetTodoOrNull(todo.ID);

                if (oldTodo == null)
                    return "Not found";

                var oldPriority = oldTodo.Priority;
                var maxPriority = await PriorityManager.GetMaximumPriority(todo.ColumnID, todoRepository);

                if (oldPriority == todo.Priority || todo.Priority < 0 || todo.Priority > maxPriority || 
                    oldTodo.Title != todo.Title || oldTodo.Description != todo.Description || 
                    oldTodo.Deadline != todo.Deadline || oldTodo.ColumnID != todo.ColumnID)
                    return "Bad request";

                var result = await todoRepository.UpdateTodo(new ModelTodo(todo.ID, todo.Title, todo.Description, todo.Deadline, maxPriority + 1, todo.ColumnID));
                if (result != "Success")
                    return "Conflict";

                if (oldPriority > todo.Priority)
                {
                    result = await PriorityManager.IncreasePriority(todo.ColumnID, oldPriority, todo.Priority, todoRepository);
                    if (result != "Success")
                        return "Conflict";
                }

                if (oldPriority < todo.Priority)
                {
                    result = await PriorityManager.ReducePriority(todo.ColumnID, oldPriority, todo.Priority, todoRepository);
                    if (result != "Success")
                        return "Conflict";
                }

                result = await todoRepository.UpdateTodo(todo);
                if (result != "Success")
                    return result;
                
                tran.Complete();
                return "Success";
            }
        }
    }

    
}
