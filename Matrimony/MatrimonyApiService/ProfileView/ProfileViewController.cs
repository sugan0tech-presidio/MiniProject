﻿using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Validations;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MatrimonyApiService.ProfileView;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowAll")]
[Authorize]
public class ProfileViewController(
    IProfileViewService profileViewService,
    IProfileService profileService,
    CustomControllerValidator validator,
    ILogger<ProfileViewController> logger)
    : ControllerBase
{
    [HttpPost("add/viewer/{viewerId}/profile/{profileId}")]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddView(int viewerId, int profileId)
    {
        try
        {
            await validator.ValidateUserPrivilegeForProfile(User.Claims, viewerId);
            await profileViewService.AddView(viewerId, profileId);
            var profile = await profileService.GetProfileById(profileId);
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
        catch (NonPremiumUserException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(403, new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
        }
        catch (ExhaustedMaximumProfileViewsException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(403, new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
        }
        catch (AuthenticationException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(403, new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
        }
        catch (InvalidProfileViewException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(400, new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> AddView(ProfileViewDto profileViewDto)
    {
        await profileViewService.AddView(profileViewDto);
        return Ok();
    }

    [HttpGet("{viewId}")]
    [ProducesResponseType(typeof(ProfileViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetViewById(int viewId)
    {
        try
        {
            var view = await profileViewService.GetViewById(viewId);
            return Ok(view);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet("profile/{profileId}")]
    [ProducesResponseType(typeof(List<ProfileViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetViewsByProfileId(int profileId)
    {
        try
        {
            var views = await profileViewService.GetViewsByProfileId(profileId);
            return Ok(views);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
        catch (NonPremiumUserException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(403, new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
        }
    }

    [HttpDelete("{viewId}")]
    [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> DeleteViewById(int viewId)
    {
        try
        {
            await profileViewService.DeleteViewById(viewId);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpDelete("before/{date}")]
    [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> DeleteOldViews(DateTime date)
    {
        try
        {
            await profileViewService.DeleteOldViews(date);
            return Ok();
        }
        catch (InvalidDateTimeException ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
        }
    }
}