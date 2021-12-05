using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelColumn = TodoManagerApp.DAL.Models.Column;

namespace TodoManagerApp.DAL.Models
{
    public interface IColumnRepository
    {
        Task<ModelColumn> GetColumnOrNull(int columnId);
        Task<IReadOnlyCollection<ModelColumn>> ColumnList();
        Task<IReadOnlyCollection<ModelColumn>> ReversedPriorityColumnList();
        Task<int> InsertColumn(ModelColumn value);
        Task<string> DeleteColumn(int columnId);
        Task<string> UpdateColumn(ModelColumn value);
    }
}
