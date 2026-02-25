namespace DataLayer.Entities
{
    public class TrainerClientEntity : BaseEntity
    {
        public Guid TrainerId { get; set; }

        public Guid UserId { get; set; }

        public DateTime AssignedAt { get; set; }
    }
}
