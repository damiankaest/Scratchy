using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scratchy.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddScratchImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScratchImageUrl",
                table: "Scratches",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScratchImageUrl",
                table: "Scratches");
        }
    }
}
