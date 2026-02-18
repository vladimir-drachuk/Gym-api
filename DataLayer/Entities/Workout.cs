using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Entities
{
    public class Workout : BaseModel
    {
        public Guid UserId;
        public Guid? WorkoutPlanId;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
