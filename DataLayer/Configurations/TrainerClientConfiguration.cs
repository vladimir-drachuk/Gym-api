using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations
{
    public class TrainerClientConfiguration : IEntityTypeConfiguration<TrainerClientEntity>
    {
        public void Configure(EntityTypeBuilder<TrainerClientEntity> builder)
        {
            builder.ToTable("TrainerClients");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.CreatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.TrainerId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.AssignedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Trainer)
                .WithMany(u => u.TrainerAssignments)
                .HasForeignKey(x => x.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Client)
                .WithMany(u => u.ClientAssignments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
