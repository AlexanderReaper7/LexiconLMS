using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LexiconLMS.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class DocumentRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Document",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModuleId",
                table: "Document",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Document_CourseId",
                table: "Document",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_ModuleId",
                table: "Document",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Courses_CourseId",
                table: "Document",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Modules_ModuleId",
                table: "Document",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Courses_CourseId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Document_Modules_ModuleId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Document_CourseId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Document_ModuleId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Document");
        }
    }
}
