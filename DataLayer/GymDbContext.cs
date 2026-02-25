using DataLayer.Configurations;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class GymDbContext(DbContextOptions<GymDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ExerciseConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfigurations());
            modelBuilder.ApplyConfiguration(new SetConfiguration());
            modelBuilder.ApplyConfiguration(new WorkoutConfiguration());
            modelBuilder.ApplyConfiguration(new WorkoutExerciseConfiguration());
            modelBuilder.ApplyConfiguration(new WorkoutPlanConfiguration());
            modelBuilder.ApplyConfiguration(new TrainerClientConfiguration());
        }

        public DbSet<ExerciseEntity> Exercises { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<SetEntity> Sets { get; set; }
        public DbSet<WorkoutEntity> Workouts { get; set; }
        public DbSet<WorkoutExerciseEntity> WorkoutExercises { get; set; }
        public DbSet<WorkoutPlanEntity> WorkoutPlans { get; set; }
        public DbSet<TrainerClientEntity> TrainerClients { get; set; }
    }
}
