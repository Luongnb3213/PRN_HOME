using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN221_Assignment.Migrations
{
    public partial class add_table_ThreadReact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserFollowErId",
                table: "Follow",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BlockUserID",
                table: "Block",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ThreadReact",
                columns: table => new
                {
                    ThreadReactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    threadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadReact", x => x.ThreadReactId);
                    table.ForeignKey(
                        name: "FK_ThreadReact_Thread_threadId",
                        column: x => x.threadId,
                        principalTable: "Thread",
                        principalColumn: "ThreadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThreadReact_threadId",
                table: "ThreadReact",
                column: "threadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThreadReact");

            migrationBuilder.DropColumn(
                name: "UserFollowErId",
                table: "Follow");

            migrationBuilder.DropColumn(
                name: "BlockUserID",
                table: "Block");
        }
    }
}
