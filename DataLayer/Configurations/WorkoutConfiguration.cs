using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations
{
    public class WorkoutConfiguration : IEntityTypeConfiguration<WorkoutEntity>
    {
        public void Configure(EntityTypeBuilder<WorkoutEntity> builder)
        {
            builder.ToTable("Workouts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.CreatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.WorkoutPlanId);
            builder.Property(x => x.Title).HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.IsCompleted).IsRequired().HasDefaultValue(false);

            builder.HasOne(x => x.User)
                .WithMany(u => u.Workouts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.WorkoutPlan)
                .WithMany(wp => wp.Workouts)
                .HasForeignKey(x => x.WorkoutPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Workout -> WorkoutExercises relationship is configured in WorkoutExerciseConfiguration
        }
    }
}
