using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoalesceSample.Data.Migrations
{
    public partial class ByteContentImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "Images",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Images");
        }
    }
}
