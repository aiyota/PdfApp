using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PdfApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateFavoritePdfs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoritePdfs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PdfId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritePdfs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoritePdfs_Pdfs_PdfId",
                        column: x => x.PdfId,
                        principalTable: "Pdfs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoritePdfs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePdfs_PdfId",
                table: "FavoritePdfs",
                column: "PdfId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePdfs_UserId",
                table: "FavoritePdfs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoritePdfs");
        }
    }
}
