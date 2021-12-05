using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using TodoManagerApp.DAL.Models;
using ModelColumn = TodoManagerApp.DAL.Models.Column;

namespace TodoManagerApp.BL
{
    public class ColumnManager
    {
        private readonly IColumnRepository columnRepository;

        public ColumnManager(IColumnRepository columnRepository)
        {
            this.columnRepository = columnRepository;
        }

        public async Task<ModelColumn> GetColumnOrNull(int columnID)
            => await columnRepository.GetColumnOrNull(columnID);

        public async Task<IReadOnlyCollection<ModelColumn>> ColumnList()
            => await columnRepository.ColumnList();

        public async Task<int> InsertColumn(ModelColumn value)
        { 
            if (value.Priority == await PriorityManager.GetMaximumPriority(columnRepository) + 1)
                return await columnRepository.InsertColumn(value);
            return -2;
        }

        public async Task<string> DeleteColumn(int id)
        {
            using (var tran = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.RepeatableRead },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                
                var column = await columnRepository.GetColumnOrNull(id);

                if (column == null)
                    return "Not found";

                var columnPriority = column.Priority;
                var columnList = await columnRepository.ColumnList();

                var result = await columnRepository.DeleteColumn(id);
                if (result != "Success")
                    return result;

                result = await PriorityManager.ReducePriority(columnPriority, -1, columnRepository);
                if (result != "Success")
                    return "Conflict";
                
                tran.Complete();
                return "Success";
            }
            
        }

        public async Task<string> UpdateColumn(ModelColumn value)
        {
            var oldColumn = await columnRepository.GetColumnOrNull(value.ID);
            if (oldColumn != null && oldColumn.Priority == value.Priority)
                return await columnRepository.UpdateColumn(value);
            
            else if (oldColumn == null)
                return "Not found";
            
            return "Bad request";
        }
        public async Task<string> ModifiedColumnPriority(ModelColumn column)
        {
            using (var tran = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.RepeatableRead },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var columnList = await columnRepository.ColumnList();
                var reversedColumnList = await columnRepository.ReversedPriorityColumnList();
                var oldColumn = await columnRepository.GetColumnOrNull(column.ID);

                if (oldColumn == null)
                    return "Not found";

                var oldPriority = oldColumn.Priority;
                var maxPriority = await PriorityManager.GetMaximumPriority(columnRepository);

                if (oldPriority == column.Priority || column.Priority < 0 ||
                    column.Priority > maxPriority || oldColumn.Name != column.Name)
                    return "Bad request";

                var result = await columnRepository.UpdateColumn(new ModelColumn(column.ID, column.Name, maxPriority + 1));
                if (result != "Success")
                    return "Conflict";

                if (oldPriority > column.Priority)
                {
                    result = await PriorityManager.IncreasePriority(oldPriority, column.Priority, columnRepository);
                    if (result != "Success")
                        return "Conflict";
                }

                if (oldPriority < column.Priority)
                {
                    result = await PriorityManager.ReducePriority(oldPriority, column.Priority, columnRepository);
                    if (result != "Success")
                        return "Conflict";
                }

                result = await columnRepository.UpdateColumn(column);
                if (result != "Success")
                    return result;
                
                tran.Complete();
                return "Success";
            }
        }

    }
}
