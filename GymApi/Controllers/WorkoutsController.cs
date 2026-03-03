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

        /// <summary>
        /// Get workouts
        /// </summary>
        /// <remarks>Retrieves workouts filtered by userId or workoutPlanId. At least one filter must be provided.</remarks>
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

        /// <summary>
        /// Get workout by ID
        /// </summary>
        /// <remarks>Retrieves a specific workout by its unique identifier.</remarks>
        [HttpGet("{workoutId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(Workout), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Workout>> GetById(Guid workoutId)
        {
            var workout = await _workoutService.GetByWorkoutId(workoutId);
            return workout == null ? NotFound() : Ok(workout);
        }

        /// <summary>
        /// Get workout exercises with sets
        /// </summary>
        /// <remarks>
        /// Returns exercises for a workout grouped into planned (with optional linked actual) and free (unplanned actual).
        /// </remarks>
        [HttpGet("{workoutId:guid}/exercises")]
        [Authorize]
        [ProducesResponseType(typeof(WorkoutExercisesResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<WorkoutExercisesResponse>> GetExercises(Guid workoutId)
        {
            var exercises = await _workoutService.GetWorkoutExercisesWithSets(workoutId);
            return exercises == null ? NotFound() : Ok(exercises);
        }

        /// <summary>
        /// Create workout
        /// </summary>
        /// <remarks>Creates a new workout with planned exercises and sets. All exercises and sets are created as planned (template) data.</remarks>
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

        /// <summary>
        /// Update workout
        /// </summary>
        /// <remarks>Updates workout metadata (description, date, isCompleted). Does not modify the plan structure.</remarks>
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

        /// <summary>
        /// Delete workout
        /// </summary>
        /// <remarks>Deletes a workout. Only allowed if no actual exercises have been executed (workout is still in plan state).</remarks>
        [HttpDelete("{workoutId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> Delete(Guid workoutId)
        {
            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutId(workoutId);
            if (!assigneeId.HasValue)
            {
                return NotFound();
            }

            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);
            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            try
            {
                await _workoutService.DeleteWorkout(workoutId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Replace workout plan
        /// </summary>
        /// <remarks>Replaces the entire workout plan with new exercises and sets. Only allowed if no actual exercises exist. Deletes all existing planned exercises and sets, then creates new ones.</remarks>
        [HttpPut("{workoutId:guid}/plan")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> ReplacePlan(Guid workoutId, [FromBody] UpdateWorkoutPlanInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutId(workoutId);
            if (!assigneeId.HasValue)
            {
                return NotFound();
            }

            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);
            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            try
            {
                await _workoutService.ReplacePlan(workoutId, input.Exercises);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Clear actual data
        /// </summary>
        /// <remarks>Removes all executed exercises and sets from the workout. The plan (planned exercises and sets) remains unchanged.</remarks>
        [HttpPost("{workoutId:guid}/clear")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> ClearActualData(Guid workoutId)
        {
            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutId(workoutId);
            if (!assigneeId.HasValue)
            {
                return NotFound();
            }

            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);
            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            try
            {
                await _workoutService.ClearActualData(workoutId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Add free exercise
        /// </summary>
        /// <remarks>Adds a free (unplanned) exercise with an actual set to the workout. Creates both the exercise and set as actual data.</remarks>
        [HttpPost("{workoutId:guid}/exercises/free")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> AddFreeExercise(Guid workoutId, [FromBody] AddFreeExerciseInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutId(workoutId);
            if (!assigneeId.HasValue)
            {
                return NotFound();
            }

            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);
            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            try
            {
                await _workoutService.AddFreeExerciseToWorkout(workoutId, input);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete free exercise
        /// </summary>
        /// <remarks>Deletes a free actual exercise (exercise that was not part of the plan). Only free exercises can be deleted.</remarks>
        [HttpDelete("exercises/{workoutExerciseId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteExercise(Guid workoutExerciseId)
        {
            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutExerciseId(workoutExerciseId);
            if (!assigneeId.HasValue)
            {
                return NotFound();
            }

            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);
            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            try
            {
                await _workoutService.DeleteWorkoutExerciseFromWorkout(workoutExerciseId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Add actual set
        /// </summary>
        /// <remarks>Adds an actual set to a planned exercise. If this is the first actual set for the exercise, creates the actual exercise automatically. The Date field is required.</remarks>
        [HttpPost("exercises/{workoutExerciseId:guid}/sets")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> AddSetToWorkoutExercise(Guid workoutExerciseId, [FromBody] AddSetInput setInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutExerciseId(workoutExerciseId);
            if (!assigneeId.HasValue)
            {
                return NotFound();
            }

            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);
            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            try
            {
                await _workoutService.AddSetToWorkoutExercise(workoutExerciseId, setInput);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update actual set
        /// </summary>
        /// <remarks>Updates an actual set. Only actual sets (with Date) can be updated.</remarks>
        [HttpPut("exercises/{workoutExerciseId:guid}/sets/{setId:guid}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponse>> UpdateSet(Guid workoutExerciseId, Guid setId, [FromBody] Set set)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutExerciseId(workoutExerciseId);
            if (!assigneeId.HasValue)
            {
                return NotFound();
            }

            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);
            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            try
            {
                set.Id = setId;
                await _workoutService.UpdateWorkoutExerciseSet(workoutExerciseId, set);
                return Ok(ApiResponse.Updated("Set"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete actual set
        /// </summary>
        /// <remarks>Deletes an actual set. Only actual sets (with Date) can be deleted.</remarks>
        [HttpDelete("exercises/{workoutExerciseId:guid}/sets/{setId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteSet(Guid workoutExerciseId, Guid setId)
        {
            var assignerId = User.GetUserId();
            var assigneeId = await _workoutService.GetUserIdByWorkoutExerciseId(workoutExerciseId);
            if (!assigneeId.HasValue)
            {
                return NotFound();
            }

            var isActivityCanAssign = await _trainerService.CheckActivityAssign(assignerId, assigneeId);
            if (isActivityCanAssign)
            {
                Forbid("You are not allowed to assign this activity to this user");
            }

            try
            {
                var found = await _workoutService.DeleteSet(setId);
                return found ? NoContent() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
