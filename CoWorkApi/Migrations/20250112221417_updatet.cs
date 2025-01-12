using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoWorkApi.Migrations
{
    /// <inheritdoc />
    public partial class updatet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogInfos_Users_UserId",
                table: "LogInfos");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "LogInfos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_LogInfos_Users_UserId",
                table: "LogInfos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogInfos_Users_UserId",
                table: "LogInfos");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "LogInfos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LogInfos_Users_UserId",
                table: "LogInfos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
