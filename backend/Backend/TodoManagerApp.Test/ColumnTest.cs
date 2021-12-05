using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TodoManagerApp.BL;
using TodoManagerApp.DAL.Models;
using Moq;

namespace TodoManagerApp.Test
{
    [TestClass]
    public class ColumnTest
    {
        [TestMethod]
        public async Task TestInsertFirstColumn()
        {
            var columnRepositoryMock = new Mock<IColumnRepository>();
            columnRepositoryMock.Setup(repo => repo.ColumnList()).ReturnsAsync(Array.Empty<Column>());
            columnRepositoryMock.Setup(repo => repo.InsertColumn(It.Is<Column>(c => c.Priority == 0))).ReturnsAsync(1);

            var vm = new ColumnManager(columnRepositoryMock.Object);

            Assert.IsTrue(await vm.InsertColumn(new Column(1, "Test", 0)) == 1);
            Assert.IsTrue(await vm.InsertColumn(new Column(1, "Test", 1)) == -2);
        }
    }
}
