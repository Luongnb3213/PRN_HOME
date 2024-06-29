using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN221_Assignment.Migrations
{
    public partial class FixMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MessageReceive_messID",
                table: "MessageReceive");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReceive_messID",
                table: "MessageReceive",
                column: "messID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MessageReceive_messID",
                table: "MessageReceive");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReceive_messID",
                table: "MessageReceive",
                column: "messID",
                unique: true);
        }
    }
}
