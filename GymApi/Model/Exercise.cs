namespace GymApi.Model
{
    public class Exercise
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }
}
