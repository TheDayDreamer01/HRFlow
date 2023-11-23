﻿using HRIS.Dtos.NotificationDto;
using HRIS.Exceptions;
using HRIS.Models;
using HRIS.Services.NotificationService;
using HRIS.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationService _notificationService;

        public NotificationController(ILogger<NotificationController> logger, INotificationService notificationService)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _notificationService = notificationService ??
                throw new ArgumentNullException(nameof(notificationService));
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var userId = UserClaim.GetCurrentUser(HttpContext) ??
                    throw new UserNotFoundException("Invalid user's credential. Please try again.");
                var response = await _notificationService.GetNotifications(userId);
                return Ok(response);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError("An error occurred while attempting to find employee.", ex);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to get positions.");
                return Problem("Internal server error.");
            }
        }

        [HttpGet("{notificationId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetNotification([FromRoute] Guid notificationId)
        {
            try
            {
                var userId = UserClaim.GetCurrentUser(HttpContext) ??
                    throw new UserNotFoundException("Invalid user's credential. Please try again.");
                var response = await _notificationService.GetNotification(userId, notificationId);
                return Ok(response);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError("An error occurred while attempting to find employee.", ex);
                return NotFound(ex.Message);
            }
            catch (NotificationNotFoundException ex)
            {
                _logger.LogError("An error occurred while attempting to find notification.", ex);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to get positions.");
                return Problem("Internal server error.");
            }
        }

        [Authorize(Roles = "Human Resource")]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto request)
        {
            try
            {
                var hrId = UserClaim.GetCurrentUser(HttpContext) ??
                    throw new UserNotFoundException("Invalid user's credential. Please try again.");
                var response = await _notificationService.CreateNotification(hrId, request);
                return Ok(response);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError("An error occurred while attempting to find employee.", ex);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to get positions.");
                return Problem("Internal server error.");
            }
        }

        [Authorize(Roles = "Human Resource")]
        [HttpPatch("{notificationId}")]
        [Consumes("application/json")]
        [Produces("application/json")]

        public async Task<IActionResult> UpdateNotification([FromRoute] Guid notificationId, [FromBody] JsonPatchDocument<Notification> request)
        {
            try
            {
                var hrId = UserClaim.GetCurrentUser(HttpContext) ??
                    throw new UserNotFoundException("Invalid user's credential. Please try again.");
                var response = await _notificationService.UpdateNotification(hrId, notificationId, request);
                if (!response)
                {
                    throw new Exception("Failed to update notification information.");
                }
                return Ok("Successfully updated notification's information.");
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError("An error occurred while attempting to find employee.", ex);
                return NotFound(ex.Message);
            }
            catch (NotificationNotFoundException ex)
            {
                _logger.LogError("An error occurred while attempting to find notification.", ex);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to get positions.");
                return Problem("Internal server error.");
            }
        }

        [Authorize(Roles = "Human Resource")]
        [HttpPut("{notificationId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateNotifications([FromRoute] Guid notificationId, [FromBody] UpdateNotificationDto request)
        {
            try
            {
                var hrId = UserClaim.GetCurrentUser(HttpContext) ??
                    throw new UserNotFoundException("Invalid user's credential. Please try again.");
                var response = await _notificationService.UpdateNotifications(hrId, notificationId, request);
                return Ok(response);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError("An error occurred while attempting to find employee.", ex);
                return NotFound(ex.Message);
            }
            catch (NotificationNotFoundException ex)
            {
                _logger.LogError("An error occurred while attempting to find notification.", ex);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to get positions.");
                return Problem("Internal server error.");
            }
        }

        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification([FromRoute] Guid notificationId)
        {
            try
            {
                var userId = UserClaim.GetCurrentUser(HttpContext) ??
                    throw new UserNotFoundException("Invalid user's credential. Please try again.");
                var response = await _notificationService.DeleteNotification(userId, notificationId);
                return Ok("Successfully deleted notification.");
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError("An error occurred while attempting to find employee.", ex);
                return NotFound(ex.Message);
            }
            catch (NotificationNotFoundException ex)
            {
                _logger.LogError("An error occurred while attempting to find notification.", ex);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to get positions.");
                return Problem("Internal server error.");
            }
        }
    }
}
