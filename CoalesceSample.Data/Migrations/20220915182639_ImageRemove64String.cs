using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoalesceSample.Data.Migrations
{
    public partial class ImageRemove64String : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Base64Image",
                table: "Images");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Content",
                table: "Images",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Content",
                table: "Images",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Base64Image",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
