using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations
{
    public class WorkoutPlanConfiguration : IEntityTypeConfiguration<WorkoutPlanEntity>
    {
        public void Configure(EntityTypeBuilder<WorkoutPlanEntity> builder)
        {
            builder.ToTable("WorkoutPlans");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.CreatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasMany<WorkoutEntity>()
                .WithOne()
                .HasForeignKey(w => w.WorkoutPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
