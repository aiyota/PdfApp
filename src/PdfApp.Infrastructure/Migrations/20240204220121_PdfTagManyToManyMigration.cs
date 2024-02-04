using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PdfApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PdfTagManyToManyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Pdfs_PdfId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_PdfId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "PdfId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "PdfTag",
                columns: table => new
                {
                    PdfId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfTag", x => new { x.PdfId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PdfTag_Pdfs_PdfId",
                        column: x => x.PdfId,
                        principalTable: "Pdfs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PdfTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PdfTag_TagsId",
                table: "PdfTag",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PdfTag");

            migrationBuilder.AddColumn<int>(
                name: "PdfId",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_PdfId",
                table: "Tags",
                column: "PdfId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Pdfs_PdfId",
                table: "Tags",
                column: "PdfId",
                principalTable: "Pdfs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
