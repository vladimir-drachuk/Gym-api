using DataLayer.Entities;
using GymApi.Extensions;
using GymApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers
{
    /// <summary>
    /// Controller for managing trainer-client relationships
    /// </summary>
    [ApiController]
    [Authorize(Roles = "Admin,Trainer")]
    [Route("api/[controller]")]
    public class TrainersController(TrainerService trainerService) : ControllerBase
    {
        private readonly TrainerService _trainerService = trainerService;

        /// <summary>
        /// Gets all clients assigned to a trainer
        /// </summary>
        /// <param name="trainerId">The trainer identifier</param>
        /// <returns>List of trainer-client relationships</returns>
        [HttpGet("{trainerId:guid}/clients")]
        [ProducesResponseType(typeof(TrainerClientEntity[]), 200)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<TrainerClientEntity[]>> GetClients(Guid trainerId)
        {
            if (!IsAllowedToManage(trainerId))
            {
                return Forbid();
            }

            var clients = await _trainerService.GetClientsAsync(trainerId);
            return Ok(clients);
        }

        /// <summary>
        /// Assigns a client to a trainer
        /// </summary>
        /// <param name="trainerId">The trainer identifier</param>
        /// <param name="userId">The client user identifier</param>
        /// <returns>No content if successful</returns>
        [HttpPost("{trainerId:guid}/assign/{userId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> AssignClient(Guid trainerId, Guid userId)
        {
            if (!IsAllowedToManage(trainerId))
            {
                return Forbid();
            }

            try
            {
                await _trainerService.AssignClientAsync(trainerId, userId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Unassigns a client from a trainer
        /// </summary>
        /// <param name="trainerId">The trainer identifier</param>
        /// <param name="userId">The client user identifier</param>
        /// <returns>No content if successful, 404 if not found</returns>
        [HttpDelete("{trainerId:guid}/clients/{userId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UnassignClient(Guid trainerId, Guid userId)
        {
            if (!IsAllowedToManage(trainerId))
            {
                return Forbid();
            }

            var found = await _trainerService.UnassignClientAsync(trainerId, userId);
            return found ? NoContent() : NotFound();
        }

        private bool IsAllowedToManage(Guid trainerId)
        {
            if (User.IsInRole("Admin"))
            {
                return true;
            }

            var callerId = User.GetUserId();
            return callerId.HasValue && callerId.Value == trainerId;
        }
    }
}
