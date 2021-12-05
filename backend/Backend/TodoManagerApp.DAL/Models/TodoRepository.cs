using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DataTodo = TodoManagerApp.DAL.Data.Todo;
using ModelTodo = TodoManagerApp.DAL.Models.Todo;

namespace TodoManagerApp.DAL.Models 
{
    internal static class TodoConverter
    {
        public static ModelTodo ConvertFromDb(DataTodo dbTodo)
        {
            if (dbTodo == null)
                return null;
            return new ModelTodo(dbTodo.ID, dbTodo.Title, dbTodo.Description, dbTodo.Deadline, dbTodo.Priority, dbTodo.ColumnID);
        }

        public static DataTodo ConvertToDb(ModelTodo todo)
        {
            DataTodo dbTodo = new DataTodo();
            dbTodo.Title = todo.Title;
            dbTodo.Description = todo.Description;
            dbTodo.Deadline = todo.Deadline;
            dbTodo.Priority = todo.Priority;
            dbTodo.ColumnID = todo.ColumnID;
            return dbTodo;
        }

        public static void UpdateExistingDbRecord(ModelTodo todo, DataTodo dbTodo)
        {
            dbTodo.Title = todo.Title;
            dbTodo.Description = todo.Description;
            dbTodo.Deadline = todo.Deadline;
            dbTodo.Priority = todo.Priority;
            dbTodo.ColumnID = todo.ColumnID;
        }
    }

    internal static class TodoRepositoryExtensions
    {
        public static IQueryable<DataTodo> SearchByColumn(this IQueryable<DataTodo> todos, int columnID)
        {
            if (columnID == -1)
                return todos;
            return todos.Where(t => t.ColumnID == columnID);
        }
    }

    public class TodoRepository : ITodoRepository
    {
        private readonly Data.DbTaskManagerContext db;
        
        public TodoRepository(Data.DbTaskManagerContext db)
        {
            this.db = db;
        }

        public async Task<ModelTodo> GetTodoOrNull(int todoID)
        {
            var dbTodo = await db.Todos.FirstOrDefaultAsync(t => t.ID == todoID);
            if (dbTodo == null)
                return null;
            return TodoConverter.ConvertFromDb(dbTodo);
        }

        public async Task<IReadOnlyCollection<ModelTodo>> TodoList(int columnID = -1)
        {
            return await db.Todos.SearchByColumn(columnID)
                .OrderBy(t => t.ColumnID)
                .OrderBy(t => t.Priority)
                .Select(dbTodo => TodoConverter.ConvertFromDb(dbTodo))
                .ToArrayAsync();
    }

        public async Task<IReadOnlyCollection<ModelTodo>> ReversedPriorityTodoList(int columnID = -1)
        {
            return await db.Todos.SearchByColumn(columnID)
                .OrderBy(t => t.ColumnID)
                .OrderByDescending(t => t.Priority)
                .Select(dbTodo => TodoConverter.ConvertFromDb(dbTodo))
                .ToArrayAsync();
        }

        public async Task<int> InsertTodo(ModelTodo value)
        {
            int retries = 3;
            while (true)
            {
                var toDbRecord = TodoConverter.ConvertToDb(value);
                db.Todos.Add(toDbRecord);

                try
                {
                    await db.SaveChangesAsync();
                    return toDbRecord.ID;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (--retries < 0)
                        return -1;

                    foreach (var e in ex.Entries)
                        await e.ReloadAsync();
                }
                catch (DbUpdateException)
                {
                    return -2;
                }
            }
        }

        public async Task<string> DeleteTodo(int todoId)
        {
            int retries = 3;
            while (true)
            {
                var dbRecord = await db.Todos.FirstOrDefaultAsync(t => t.ID == todoId);
                if (dbRecord == null)
                    return "Not found";
                db.Todos.Remove(dbRecord);

                try
                {
                    await db.SaveChangesAsync();
                    return "Success";
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (--retries < 0)
                        return "Conflict";

                    foreach (var e in ex.Entries)
                        await e.ReloadAsync();
                }
            }
        }

        public async Task<string> UpdateTodo(ModelTodo value)
        {
            int retries = 3;
            while (true)
            {
                var dbRecord = await db.Todos.FirstOrDefaultAsync(c => c.ID == value.ID);
                if (dbRecord == null)
                    return "Not found";

                TodoConverter.UpdateExistingDbRecord(value, dbRecord);
                try
                {
                    await db.SaveChangesAsync();
                    return "Success";
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (--retries < 0)
                        return "Conflict";

                    foreach (var e in ex.Entries)
                        await e.ReloadAsync();
                }
                catch (DbUpdateException)
                {
                    return "Bad request";
                }
            }
        }

    }
}
