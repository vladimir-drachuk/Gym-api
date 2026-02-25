namespace DataLayer.Entities
{
    public class WorkoutPlanEntity : BaseEntity
    {
        public Guid UserId { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
