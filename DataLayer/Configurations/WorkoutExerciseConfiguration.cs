using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations
{
    public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutExerciseEntity>
    {
        public void Configure(EntityTypeBuilder<WorkoutExerciseEntity> builder)
        {
            builder.ToTable("WorkoutExercises");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.CreatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.WorkoutId).IsRequired();
            builder.Property(x => x.PlannedWorkoutExerciseId);
            builder.Property(x => x.ExerciseId).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Order).IsRequired();
            builder.Property(x => x.Date);

            builder.HasOne(x => x.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(x => x.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Exercise)
                .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(x => x.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Self-referential: actual exercise references its planned counterpart
            builder.HasOne(x => x.PlannedWorkoutExercise)
                .WithMany()
                .HasForeignKey(x => x.PlannedWorkoutExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            // WorkoutExercise -> Sets relationship is configured in SetConfiguration
        }
    }
}
