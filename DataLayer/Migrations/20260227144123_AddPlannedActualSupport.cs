using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddPlannedActualSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Workouts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "WorkoutExercises",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlannedWorkoutExerciseId",
                table: "WorkoutExercises",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Sets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlannedSetId",
                table: "Sets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_PlannedWorkoutExerciseId",
                table: "WorkoutExercises",
                column: "PlannedWorkoutExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_PlannedSetId",
                table: "Sets",
                column: "PlannedSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_Sets_PlannedSetId",
                table: "Sets",
                column: "PlannedSetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_WorkoutExercises_PlannedWorkoutExerciseId",
                table: "WorkoutExercises",
                column: "PlannedWorkoutExerciseId",
                principalTable: "WorkoutExercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sets_Sets_PlannedSetId",
                table: "Sets");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_WorkoutExercises_PlannedWorkoutExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutExercises_PlannedWorkoutExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropIndex(
                name: "IX_Sets_PlannedSetId",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "WorkoutExercises");

            migrationBuilder.DropColumn(
                name: "PlannedWorkoutExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "PlannedSetId",
                table: "Sets");
        }
    }
}
