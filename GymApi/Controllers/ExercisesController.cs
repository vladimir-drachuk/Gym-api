using Microsoft.AspNetCore.Mvc;
using GymApi.Services;
using GymApi.Model;
using Microsoft.AspNetCore.Authorization;

namespace GymApi.Controllers
{
    /// <summary>
    /// Controller for managing exercises
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Trainer")]
    public class ExercisesController(ExerciseService exerciseService) : ControllerBase
    {
        private readonly ExerciseService _exerciseService = exerciseService;

        /// <summary>
        /// Gets all exercises
        /// </summary>
        /// <returns>List of all exercises</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(Exercise[]), 200)]
        public async Task<ActionResult<Exercise[]>> Get()
        {
            var exercises = await _exerciseService.GetAllExercises();
            return Ok(exercises);
        }

        /// <summary>
        /// Gets an exercise by its identifier
        /// </summary>
        /// <param name="id">The exercise identifier</param>
        /// <returns>The exercise if found, otherwise 404</returns>
        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(Exercise), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Exercise?>> GetById(Guid id)
        {
            var exercise = await _exerciseService.GetExerciseById(id);
            
            if (exercise == null)
            {
                return NotFound();
            }
            
            return Ok(exercise);
        }

        /// <summary>
        /// Creates a new exercise
        /// </summary>
        /// <param name="exercise">The exercise data to create</param>
        /// <returns>The created exercise</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Exercise), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Exercise>> Create([FromBody] Exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdExercise = await _exerciseService.CreateExercise(exercise);
            return CreatedAtAction(nameof(GetById), new { id = createdExercise.Id }, createdExercise);
        }

        /// <summary>
        /// Updates an existing exercise
        /// </summary>
        /// <param name="exercise">The exercise data to update</param>
        /// <returns>The updated exercise</returns>
        [HttpPut]
        [ProducesResponseType(typeof(Exercise), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Exercise>> Update([FromBody] Exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedExercise = await _exerciseService.UpdateExercise(exercise);
            return Ok(updatedExercise);
        }

        /// <summary>
        /// Deletes an exercise by its identifier
        /// </summary>
        /// <param name="id">The exercise identifier</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _exerciseService.RemoveExercise(id);
            return NoContent();
        }
    }
}
