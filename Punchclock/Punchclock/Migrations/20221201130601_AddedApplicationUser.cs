using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Punchclock.Migrations
{
    /// <inheritdoc />
    public partial class AddedApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApplicationUserId",
                table: "Entries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "BLOB", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_ApplicationUserId",
                table: "Entries",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Users_ApplicationUserId",
                table: "Entries",
                column: "ApplicationUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Users_ApplicationUserId",
                table: "Entries");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Entries_ApplicationUserId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Entries");
        }
    }
}
