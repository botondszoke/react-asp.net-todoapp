using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataColumn = TodoManagerApp.DAL.Data.Column;
using ModelColumn = TodoManagerApp.DAL.Models.Column;
using DataTodo = TodoManagerApp.DAL.Data.Todo;

namespace TodoManagerApp.DAL.Models
{
    internal class ColumnConverter
    {
        public static ModelColumn ConvertFromDb(DataColumn dbColumn)
        {
            if (dbColumn == null)
                return null;
            return new ModelColumn(dbColumn.ID, dbColumn.Name, dbColumn.Priority);
        }

        public static DataColumn ConvertToDb(ModelColumn column)
        {
            DataColumn dbColumn = new DataColumn();
            dbColumn.Name = column.Name;
            dbColumn.Priority = column.Priority;
            dbColumn.Todos = new List<DataTodo>();
            return dbColumn;
        }

        public static void ConvertToExistingDbRecord(ModelColumn column, DataColumn dbColumn)
        {
            dbColumn.Name = column.Name;
            dbColumn.Priority = column.Priority;
        }
    }



    public class ColumnRepository : IColumnRepository
    {
        private readonly Data.DbTaskManagerContext db;

        public ColumnRepository(Data.DbTaskManagerContext db)
        {
            this.db = db;
        }

        public async Task<ModelColumn> GetColumnOrNull(int columnId)
        {
            var dbColumn = await db.Columns.FirstOrDefaultAsync(c => c.ID == columnId);
            if (dbColumn == null)
                return null;
            return ColumnConverter.ConvertFromDb(dbColumn);
        }

        public async Task<IReadOnlyCollection<ModelColumn>> ColumnList()
        {
            return await db.Columns
                .OrderBy(c => c.Priority)
                .Select(dbColumn => ColumnConverter.ConvertFromDb(dbColumn))
                .ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<ModelColumn>> ReversedPriorityColumnList()
        {
            return await db.Columns
                .OrderByDescending(c => c.Priority)
                .Select(dbColumn => ColumnConverter.ConvertFromDb(dbColumn))
                .ToArrayAsync();
        }

        public async Task<int> InsertColumn(ModelColumn value)
        {
            int retries = 3;
            while (true)
            {
                var toDbRecord = ColumnConverter.ConvertToDb(value);
                db.Columns.Add(toDbRecord);

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

        public async Task<string> DeleteColumn(int columnId)      
        {
            int retries = 3;
            while (true)
            {
                var dbRecord = await db.Columns.FirstOrDefaultAsync(c => c.ID == columnId);
                if (dbRecord == null)
                    return "Not found";
                db.Columns.Remove(dbRecord);

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
        
        public async Task<string> UpdateColumn(ModelColumn value)
        {
            int retries = 3;
            while (true)
            {
                var dbRecord = await db.Columns.FirstOrDefaultAsync(c => c.ID == value.ID);
                if (dbRecord == null)
                    return "Not found";

                ColumnConverter.ConvertToExistingDbRecord(value, dbRecord);
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
