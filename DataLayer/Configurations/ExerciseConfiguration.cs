using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<ExerciseEntity>
    {
        public void Configure(EntityTypeBuilder<ExerciseEntity> builder)
        {
            builder.ToTable("Exercises");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.CreatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description);

            // Exercise -> WorkoutExercises relationship is configured in WorkoutExerciseConfiguration
        }
    }
}
