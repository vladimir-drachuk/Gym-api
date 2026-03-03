using GymApi.Model;
using GymApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers
{
    /// <summary>
    /// Controller for managing workout plans
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Trainer")]
    public class WorkoutPlansController(WorkoutPlanService workoutPlanService) : ControllerBase
    {
        private readonly WorkoutPlanService _workoutPlanService = workoutPlanService;

        /// <summary>
        /// Gets all workout plans
        /// </summary>
        /// <returns>List of all workout plans</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<WorkoutPlan>), 200)]
        public async Task<ActionResult<List<WorkoutPlan>>> GetAll()
        {
            var plans = await _workoutPlanService.GetAll();
            return Ok(plans);
        }

        /// <summary>
        /// Gets a workout plan by its identifier
        /// </summary>
        /// <param name="id">The workout plan identifier</param>
        /// <returns>The workout plan if found, otherwise 404</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WorkoutPlan), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<WorkoutPlan>> GetById(Guid id)
        {
            var plan = await _workoutPlanService.GetById(id);
            return plan == null ? NotFound() : Ok(plan);
        }

        /// <summary>
        /// Creates a new workout plan
        /// </summary>
        /// <param name="createWorkoutPlan">The workout plan data to create</param>
        /// <returns>The created workout plan</returns>
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

        /// <summary>
        /// Updates an existing workout plan
        /// </summary>
        /// <param name="id">The workout plan identifier</param>
        /// <param name="updateWorkoutPlan">The workout plan data to update</param>
        /// <returns>The updated workout plan if found, otherwise 404</returns>
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

        /// <summary>
        /// Deletes a workout plan by its identifier
        /// </summary>
        /// <param name="id">The workout plan identifier</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _workoutPlanService.Delete(id);
            return NoContent();
        }
    }
}
