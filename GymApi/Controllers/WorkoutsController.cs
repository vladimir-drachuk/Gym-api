using FluentValidation;
using GymApi.Model;
using GymApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GymApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutsController(
        WorkoutService workoutService,
        IValidator<CreateWorkout> createWorkoutValidator) : ControllerBase
    {
        private readonly WorkoutService _workoutService = workoutService;
        private readonly IValidator<CreateWorkout> _createWorkoutValidator = createWorkoutValidator;

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(Workout[]), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Workout[]?>> GetAll(
            [FromQuery] Guid? userId,
            [FromQuery] Guid? workoutPlanId
        )
        {
            if (userId.HasValue)
            {
                var workouts = await _workoutService.GetAllByUserId(userId.Value);      
                return Ok(workouts);
            }

            if (workoutPlanId.HasValue)
            {
                var workouts = await _workoutService.GetAllByWorkoutPlanId(workoutPlanId.Value);
                return Ok(workouts);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("{workoutId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(Workout), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Workout>> GetById(Guid workoutId)
        {
            var workout = await _workoutService.GetByWorkoutId(workoutId);
            return workout == null ? NotFound() : Ok(workout);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Workout), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Workout[]?>> CreateWorkout([FromBody] CreateWorkout createWorkout)
        {
            var validationResult = await _createWorkoutValidator.ValidateAsync(createWorkout);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage }));

            try
            {
                var workout = await _workoutService.CreateWorkout(createWorkout);
                return Ok(workout);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{workoutId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(Workout), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Workout>> UpdateWorkout(Guid workoutId, [FromBody] UpdateWorkout updateWorkout)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _workoutService.UpdateWorkout(workoutId, updateWorkout);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{workoutId:guid}")]
        [Authorize]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(Guid workoutId)
        {
            await _workoutService.DeleteWorkout(workoutId);
            return NoContent();
        }

        [HttpPost("{workoutId:guid}/exercises")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> AddExercises(Guid workoutId, [FromBody] WorkoutExerciseInput[] exercises)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _workoutService.AddExercisesToWorkout(workoutId, exercises);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("exercises/{workoutExerciseId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponse>> UpdateExercise(Guid workoutExerciseId, [FromBody] UpdateWorkoutExercise updateWorkoutExercise)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var found = await _workoutService.UpdateWorkoutExercise(workoutExerciseId, updateWorkoutExercise);
            return found ? Ok(ApiResponse.Updated("Workout exercise")) : NotFound();
        }

        [HttpDelete("exercises/{workoutExerciseId:guid}")]
        [Authorize]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteExercise(Guid workoutExerciseId)
        {
            await _workoutService.DeleteWorkoutExerciseFromWorkout(workoutExerciseId);
            return NoContent();
        }

        [HttpPost("exercises/{workoutExerciseId:guid}/sets")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddSetToWorkoutExercise(Guid workoutExerciseId, [FromBody] Set set)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _workoutService.AddSetToWorkoutExercise(workoutExerciseId, set);
            return NoContent();
        }

        [HttpPut("exercises/{workoutExerciseId:guid}/sets/{setId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponse>> UpdateSet(Guid workoutExerciseId, Guid setId, [FromBody] Set set)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            set.Id = setId;
            await _workoutService.UpdateWorkoutExerciseSet(workoutExerciseId, set);
            return Ok(ApiResponse.Updated("Set"));
        }

        [HttpDelete("exercises/{workoutExerciseId:guid}/sets/{setId:guid}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteSet(Guid workoutExerciseId, Guid setId)
        {
            var found = await _workoutService.DeleteSet(setId);
            return found ? NoContent() : NotFound();
        }
    }
}
