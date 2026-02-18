using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Entities
{
    public class Set : BaseModel
    {
        public Guid WorkoutExerciseId { get; set; }

        public int Amount { get; set; } = 0;

        public int? Time { get; set; }

        public int[]? Cheating { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
