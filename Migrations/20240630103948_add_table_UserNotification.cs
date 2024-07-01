using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN221_Assignment.Migrations
{
    public partial class add_table_UserNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserNofication",
                columns: table => new
                {
                    UserNoficationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NoficationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNofication", x => x.UserNoficationId);
                    table.ForeignKey(
                        name: "FK_UserNofication_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserNofication_Nofication_NoficationId",
                        column: x => x.NoficationId,
                        principalTable: "Nofication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserNofication_NoficationId",
                table: "UserNofication",
                column: "NoficationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNofication_UserId",
                table: "UserNofication",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserNofication");
        }
    }
}
