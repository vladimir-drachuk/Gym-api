using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations
{
    public class SetConfiguration : IEntityTypeConfiguration<SetEntity>
    {
        public void Configure(EntityTypeBuilder<SetEntity> builder)
        {
            builder.ToTable("Sets");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.CreatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.WorkoutExerciseId).IsRequired();
            builder.Property(x => x.PlannedSetId);
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Target).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Order).IsRequired();
            builder.Property(x => x.Date);

            builder.HasOne(x => x.WorkoutExercise)
                .WithMany(we => we.Sets)
                .HasForeignKey(x => x.WorkoutExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Self-referential: actual set references its planned counterpart
            builder.HasOne(x => x.PlannedSet)
                .WithMany()
                .HasForeignKey(x => x.PlannedSetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
