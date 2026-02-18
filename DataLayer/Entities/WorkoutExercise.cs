using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Entities
{
    public class WorkoutExercise : BaseModel
    {
        public Guid WorkoutId { get; set; }

        public Guid ExerciseId { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
