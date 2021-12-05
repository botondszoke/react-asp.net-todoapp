using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoManagerApi.Migrations
{
    public partial class UniquePriorityInColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Todo_Priority",
                table: "Todo");

            migrationBuilder.CreateIndex(
                name: "IX_Todo_Priority_ColumnID",
                table: "Todo",
                columns: new[] { "Priority", "ColumnID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Todo_Priority_ColumnID",
                table: "Todo");

            migrationBuilder.CreateIndex(
                name: "IX_Todo_Priority",
                table: "Todo",
                column: "Priority",
                unique: true);
        }
    }
}
