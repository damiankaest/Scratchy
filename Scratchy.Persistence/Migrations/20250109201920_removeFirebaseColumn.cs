using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scratchy.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeFirebaseColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirebaseId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FirebaseId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
