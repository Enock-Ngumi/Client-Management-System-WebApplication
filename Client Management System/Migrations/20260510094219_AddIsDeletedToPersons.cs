using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToPersons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dob",
                table: "persons",
                newName: "dateofbirth");

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateofbirth",
                table: "persons",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "persons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "persons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "persons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "persons",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "persons");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "persons");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "persons");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "persons");

            migrationBuilder.RenameColumn(
                name: "dateofbirth",
                table: "persons",
                newName: "Dob");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Dob",
                table: "persons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
