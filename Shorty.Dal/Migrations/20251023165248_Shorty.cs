using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shorty.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Shorty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {

            migrationBuilder.AddColumn<DateTime>(
                     name: "CreatedAt",
                     table: "Users",
                     type: "timestamp without time zone",
                     nullable: false,
                     defaultValueSql: "now() at time zone 'utc'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(name: "CreatedAt", table: "Users");
        }
    }
}
