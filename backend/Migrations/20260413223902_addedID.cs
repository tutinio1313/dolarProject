using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class addedID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_exchanges",
                table: "exchanges");

            migrationBuilder.AddColumn<string>(
                name: "ID",
                table: "exchanges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_exchanges",
                table: "exchanges",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_exchanges",
                table: "exchanges");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "exchanges");

            migrationBuilder.AddPrimaryKey(
                name: "PK_exchanges",
                table: "exchanges",
                column: "ExchangeID");
        }
    }
}
