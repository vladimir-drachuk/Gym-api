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
            builder.Property(x => x.ExerciseId).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Order).IsRequired();

            builder.HasMany<SetEntity>()
                .WithOne()
                .HasForeignKey(s => s.WorkoutExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
