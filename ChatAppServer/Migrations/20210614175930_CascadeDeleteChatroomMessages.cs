using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatAppServer.Migrations
{
    public partial class CascadeDeleteChatroomMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatroomMessage_Chatroom_ChatroomId",
                table: "ChatroomMessage");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatroomMessage_Chatroom_ChatroomId",
                table: "ChatroomMessage",
                column: "ChatroomId",
                principalTable: "Chatroom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatroomMessage_Chatroom_ChatroomId",
                table: "ChatroomMessage");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatroomMessage_Chatroom_ChatroomId",
                table: "ChatroomMessage",
                column: "ChatroomId",
                principalTable: "Chatroom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
