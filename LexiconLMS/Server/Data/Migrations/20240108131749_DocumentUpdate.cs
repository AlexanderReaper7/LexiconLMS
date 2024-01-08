using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LexiconLMS.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class DocumentUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Courses_CourseId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Modules_ModuleId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Documents");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityId",
                table: "Documents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleOfUploader",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Courses_CourseId",
                table: "Documents",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Modules_ModuleId",
                table: "Documents",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Courses_CourseId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Modules_ModuleId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "RoleOfUploader",
                table: "Documents");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityId",
                table: "Documents",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Courses_CourseId",
                table: "Documents",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Modules_ModuleId",
                table: "Documents",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
