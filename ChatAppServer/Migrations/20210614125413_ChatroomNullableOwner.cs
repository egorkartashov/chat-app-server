using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatAppServer.Migrations
{
    public partial class ChatroomNullableOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chatroom_User_OwnerId",
                table: "Chatroom");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Chatroom",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Chatroom_User_OwnerId",
                table: "Chatroom",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chatroom_User_OwnerId",
                table: "Chatroom");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Chatroom",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chatroom_User_OwnerId",
                table: "Chatroom",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
