using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Entities
{
    public class WorkoutPlan : BaseModel
    {
        public Guid UserId;

        public string Description { get; set; } = string.Empty;
    }
}
