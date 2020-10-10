using FluentMigrator;

namespace KanbanBoard.Migrations
{
    [Migration(202010101800)]
    public class SetupDatabase : Migration
    {
        public override void Down()
        {
            Delete.Table("assignments");
            Delete.Table("board_members");
            Delete.Table("list_tasks");
            Delete.Table("tasks");
            Delete.Table("lists");
            Delete.Table("boards");
            Delete.Table("users");
        }

        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsSerial().PrimaryKey("PK_users_id")
                .WithColumn("name").AsString(100).NotNullable()
                .WithColumn("email").AsString(320).Unique().NotNullable()
                .WithColumn("password").AsString(75).NotNullable()
                .WithColumn("created_on").AsTimestamp2().NotNullable()
                .WithColumn("modified_on").AsTimestamp2().NotNullable();
            Create.Table("boards")
                .WithColumn("id").AsSerial().PrimaryKey("PK_boards_id")
                .WithColumn("title").AsString(150).NotNullable()
                .WithColumn("created_by").AsInt32().NotNullable().ForeignKey("FK_boards_created_by", "users", "id")
                .WithColumn("created_on").AsTimestamp2().NotNullable()
                .WithColumn("modified_on").AsTimestamp2().NotNullable();
            Create.Table("board_members")
                .WithColumn("user_id").AsInt32().NotNullable().ForeignKey("FK_board_members_user_id", "users", "id")
                .WithColumn("board_id").AsInt32().NotNullable().ForeignKey("FK_board_members_board_id", "boards", "id")
                .WithColumn("is_admin").AsBoolean().NotNullable()
                .WithColumn("created_on").AsTimestamp2().NotNullable()
                .WithColumn("modified_on").AsTimestamp2().NotNullable();
            Create.PrimaryKey("PK_board_members_user_id_board_id").OnTable("board_members").Columns("user_id", "board_id");
            Create.Table("lists")
                .WithColumn("id").AsSerial().PrimaryKey("PK_lists_id")
                .WithColumn("board_id").AsInt32().NotNullable().ForeignKey("FK_lists_board_id", "boards", "id")
                .WithColumn("title").AsString(150).NotNullable()
                .WithColumn("created_on").AsTimestamp2().NotNullable()
                .WithColumn("modified_on").AsTimestamp2().NotNullable();
            Create.Table("tasks")
                .WithColumn("id").AsSerial().PrimaryKey("PK_tasks_id")
                .WithColumn("board_id").AsInt32().NotNullable().ForeignKey("FK_tasks_board_id", "boards", "id")
                .WithColumn("summary").AsText().NotNullable()
                .WithColumn("description").AsText().NotNullable()
                .WithColumn("tag_color").AsString(6).NotNullable()
                .WithColumn("created_on").AsTimestamp2().NotNullable()
                .WithColumn("modified_on").AsTimestamp2().NotNullable();
            Create.Table("list_tasks")
                .WithColumn("list_id").AsInt32().NotNullable().ForeignKey("FK_list_tasks_listId", "lists", "id")
                .WithColumn("task_id").AsInt32().NotNullable().ForeignKey("FK_list_tasks_taskId", "tasks", "id");
            Create.PrimaryKey("PK_list_tasks_list_id_task_id").OnTable("list_tasks").Columns("list_id", "task_id");
            Create.Table("assignments")
                .WithColumn("user_id").AsInt32().NotNullable()
                .WithColumn("board_id").AsInt32().NotNullable()
                .WithColumn("task_id").AsInt32().NotNullable().ForeignKey("FK_assignments_taskId", "tasks", "id");
            Create.PrimaryKey("PK_assignments_user_id_board_id_task_id")
                .OnTable("assignments").Columns("user_id", "board_id", "task_id");
            Create.ForeignKey("FK_assignments_user_id_board_id")
                .FromTable("assignments").ForeignColumns("user_id", "board_id")
                .ToTable("board_members").PrimaryColumns("user_id", "board_id");
        }
    }
}
