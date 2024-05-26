using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrugScanner.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddColsToDrug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsScanned",
                schema: "dbo",
                table: "Drugs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NoOfScans",
                schema: "dbo",
                table: "Drugs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsScanned",
                schema: "dbo",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "NoOfScans",
                schema: "dbo",
                table: "Drugs");
        }
    }
}
