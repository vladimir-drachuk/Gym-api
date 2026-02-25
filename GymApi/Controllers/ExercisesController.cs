using Microsoft.AspNetCore.Mvc;
using GymApi.Services;
using GymApi.Model;
using Microsoft.AspNetCore.Authorization;

namespace GymApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Trainer")]
    public class ExercisesController(ExerciseService exerciseService) : ControllerBase
    {
        private readonly ExerciseService _exerciseService = exerciseService;

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(Exercise[]), 200)]
        public async Task<ActionResult<Exercise[]>> Get()
        {
            var exercises = await _exerciseService.GetAllExercises();
            return Ok(exercises);
        }

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
