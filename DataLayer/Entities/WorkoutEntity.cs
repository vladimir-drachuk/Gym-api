namespace DataLayer.Entities
{
    public class WorkoutEntity : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid? WorkoutPlanId { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
