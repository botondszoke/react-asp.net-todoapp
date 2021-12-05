using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoManagerApi.Migrations
{
    public partial class RemoveTaskName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Todo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Todo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
