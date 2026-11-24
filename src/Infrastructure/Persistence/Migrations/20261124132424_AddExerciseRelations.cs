using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_exercises_name",
                table: "exercises");

            migrationBuilder.DropColumn(
                name: "name",
                table: "exercises");

            migrationBuilder.DropColumn(
                name: "target_muscle_group",
                table: "exercises");

            migrationBuilder.AddColumn<string>(
                name: "attributes",
                table: "exercises",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "exercises",
                type: "varchar(500)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "exercises",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "exercises",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "exercise_images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    original_name = table.Column<string>(type: "varchar(255)", nullable: false),
                    exercise_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exercise_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_exercise_images_exercises_exercise_id",
                        column: x => x.exercise_id,
                        principalTable: "exercises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "muscle_groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_muscle_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "exercise_muscle_groups",
                columns: table => new
                {
                    exercise_id = table.Column<Guid>(type: "uuid", nullable: false),
                    muscle_group_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exercise_muscle_groups", x => new { x.exercise_id, x.muscle_group_id });
                    table.ForeignKey(
                        name: "fk_exercise_muscle_groups_exercises_exercise_id",
                        column: x => x.exercise_id,
                        principalTable: "exercises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exercise_muscle_groups_muscle_groups_muscle_group_id",
                        column: x => x.muscle_group_id,
                        principalTable: "muscle_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_exercises_title",
                table: "exercises",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_exercise_images_exercise_id",
                table: "exercise_images",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "ix_exercise_muscle_groups_muscle_group_id",
                table: "exercise_muscle_groups",
                column: "muscle_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_muscle_groups_name",
                table: "muscle_groups",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exercise_images");

            migrationBuilder.DropTable(
                name: "exercise_muscle_groups");

            migrationBuilder.DropTable(
                name: "muscle_groups");

            migrationBuilder.DropIndex(
                name: "ix_exercises_title",
                table: "exercises");

            migrationBuilder.DropColumn(
                name: "attributes",
                table: "exercises");

            migrationBuilder.DropColumn(
                name: "description",
                table: "exercises");

            migrationBuilder.DropColumn(
                name: "title",
                table: "exercises");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "exercises");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "exercises",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "target_muscle_group",
                table: "exercises",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_exercises_name",
                table: "exercises",
                column: "name",
                unique: true);
        }
    }
}
