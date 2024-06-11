using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN221_Assignment.Migrations
{
    public partial class AddDeleteTriggerForComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_DeleteComment
                ON Comment
                AFTER DELETE
                AS
                BEGIN
                    DELETE FROM ThreadComment WHERE CommentId IN (SELECT CommentId FROM DELETED);
                END
            "); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER trg_DeleteComment");
        }
    }
}
