using GymApi.Model;
using GymApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkoutPlansController(WorkoutPlanService workoutPlanService) : ControllerBase
    {
        private readonly WorkoutPlanService _workoutPlanService = workoutPlanService;

        [HttpGet]
        [ProducesResponseType(typeof(List<WorkoutPlan>), 200)]
        public async Task<ActionResult<List<WorkoutPlan>>> GetAll()
        {
            var plans = await _workoutPlanService.GetAll();
            return Ok(plans);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WorkoutPlan), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<WorkoutPlan>> GetById(Guid id)
        {
            var plan = await _workoutPlanService.GetById(id);
            return plan == null ? NotFound() : Ok(plan);
        }

        [HttpPost]
        [ProducesResponseType(typeof(WorkoutPlan), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<WorkoutPlan>> Create([FromBody] CreateWorkoutPlan createWorkoutPlan)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _workoutPlanService.Create(createWorkoutPlan);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(WorkoutPlan), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<WorkoutPlan>> Update(Guid id, [FromBody] UpdateWorkoutPlan updateWorkoutPlan)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _workoutPlanService.Update(id, updateWorkoutPlan);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _workoutPlanService.Delete(id);
            return NoContent();
        }
    }
}
