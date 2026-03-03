namespace DataLayer.Entities;

public class ExerciseEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public virtual ICollection<WorkoutExerciseEntity> WorkoutExercises { get; set; } = new List<WorkoutExerciseEntity>();
}
