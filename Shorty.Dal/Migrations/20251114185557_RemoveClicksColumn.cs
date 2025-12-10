using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shorty.Dal.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClicksColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Clicks",
                table: "Shorties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Clicks",
                table: "Shorties",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
