using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoManagerApi.Migrations
{
    public partial class UniquePriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Todo_Priority",
                table: "Todo",
                column: "Priority",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Column_Priority",
                table: "Column",
                column: "Priority",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Todo_Priority",
                table: "Todo");

            migrationBuilder.DropIndex(
                name: "IX_Column_Priority",
                table: "Column");
        }
    }
}
