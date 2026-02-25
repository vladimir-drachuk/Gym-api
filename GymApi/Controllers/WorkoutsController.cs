using GymApi.Extensions;
using GymApi.Model;
using GymApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,Trainer")]
    [Route("api/[controller]")]
    public class WorkoutsController(WorkoutService workoutService, TrainerService trainerService) : ControllerBase
    {
        private readonly WorkoutService _workoutService = workoutService;
        private readonly TrainerService _trainerService = trainerService;

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
        [ProducesResponseType(typeof(Workout), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<Workout>> CreateWorkout([FromBody] CreateWorkout createWorkout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.GetUserId();
            var isActivityCanAssign = await _trainerService.CheckActivityAssign(userId, createWorkout.UserId);

            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

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
        [ProducesResponseType(typeof(Workout), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Workout>> UpdateWorkout(Guid workoutId, [FromBody] UpdateWorkout updateWorkout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.GetUserId();
            var isActivityCanAssign = await _trainerService.CheckActivityAssign(userId, updateWorkout.UserId);

            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            var result = await _workoutService.UpdateWorkout(workoutId, updateWorkout);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{workoutId:guid}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(Guid workoutId)
        {
            await _workoutService.DeleteWorkout(workoutId);
            return NoContent();
        }

        [HttpPost("{workoutId:guid}/exercises")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> AddExercises(Guid workoutId, [FromBody] WorkoutExerciseInput[] exercises)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutId(workoutId);
            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);

            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

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
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponse>> UpdateExercise(Guid workoutExerciseId, [FromBody] UpdateWorkoutExercise updateWorkoutExercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutExerciseId(workoutExerciseId);
            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);

            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            var found = await _workoutService.UpdateWorkoutExercise(workoutExerciseId, updateWorkoutExercise);
            return found ? Ok(ApiResponse.Updated("Workout exercise")) : NotFound();
        }

        [HttpPut("exercises/{workoutExerciseId:guid}/order")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponse>> ReorderExercise(Guid workoutExerciseId, [FromBody] ReorderWorkoutExercise reorder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutExerciseId(workoutExerciseId);
            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);

            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            var found = await _workoutService.ReorderWorkoutExerciseAsync(workoutExerciseId, reorder.NewOrder);
            return found ? Ok(ApiResponse.Updated("Workout exercise order")) : NotFound();
        }

        [HttpDelete("exercises/{workoutExerciseId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> DeleteExercise(Guid workoutExerciseId)
        {
            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutExerciseId(workoutExerciseId);
            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);

            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            await _workoutService.DeleteWorkoutExerciseFromWorkout(workoutExerciseId);

            return NoContent();
        }

        [HttpPost("exercises/{workoutExerciseId:guid}/sets")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> AddSetToWorkoutExercise(Guid workoutExerciseId, [FromBody] Set set)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutExerciseId(workoutExerciseId);
            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);

            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            await _workoutService.AddSetToWorkoutExercise(workoutExerciseId, set);
            return NoContent();
        }

        [HttpPut("exercises/{workoutExerciseId:guid}/sets/{setId:guid}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ApiResponse>> UpdateSet(Guid workoutExerciseId, Guid setId, [FromBody] Set set)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutExerciseId(workoutExerciseId);
            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);

            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            set.Id = setId;
            await _workoutService.UpdateWorkoutExerciseSet(workoutExerciseId, set);
            return Ok(ApiResponse.Updated("Set"));
        }

        [HttpDelete("exercises/{workoutExerciseId:guid}/sets/{setId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteSet(Guid workoutExerciseId, Guid setId)
        {
            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutExerciseId(workoutExerciseId);
            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);

            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            var found = await _workoutService.DeleteSet(setId);
            return found ? NoContent() : NotFound();
        }
    }
}
