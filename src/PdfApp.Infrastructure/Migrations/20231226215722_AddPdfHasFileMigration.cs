using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PdfApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPdfHasFileMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasFile",
                table: "Pdfs",
                type: "bit",
                nullable: false, 
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasFile",
                table: "Pdfs");
        }
    }
}
