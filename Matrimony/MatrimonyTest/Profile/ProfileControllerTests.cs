﻿using MatrimonyApiService.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Profile;

public class ProfileControllerTests
{
    private Mock<IProfileService> _profileServiceMock;
    private Mock<ILogger<ProfileController>> _loggerMock;
    private ProfileController _profileController;

    [SetUp]
    public void SetUp()
    {
        _profileServiceMock = new Mock<IProfileService>();
        _loggerMock = new Mock<ILogger<ProfileController>>();
        _profileController = new ProfileController(_profileServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetProfileById_ReturnsOk_WhenProfileExists()
    {
        // Arrange
        var profileId = 1;
        var profileDto = new ProfileDto { ProfileId = profileId };
        _profileServiceMock.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(profileDto);

        // Act
        var result = await _profileController.GetProfileById(profileId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profileDto, result.Value);
    }

    [Test]
    public async Task GetProfileById_ReturnsNotFound_WhenProfileDoesNotExist()
    {
        // Arrange
        var profileId = 1;
        _profileServiceMock.Setup(service => service.GetProfileById(profileId)).ThrowsAsync(new KeyNotFoundException("Profile not found"));

        // Act
        var result = await _profileController.GetProfileById(profileId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetProfileByUserId_ReturnsOk_WhenProfileExists()
    {
        // Arrange
        var userId = 1;
        var profileDto = new ProfileDto { UserId = userId };
        _profileServiceMock.Setup(service => service.GetProfileByUserId(userId)).ReturnsAsync(profileDto);

        // Act
        var result = await _profileController.GetProfileByUserId(userId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profileDto, result.Value);
    }

    [Test]
    public async Task GetProfileByUserId_ReturnsNotFound_WhenProfileDoesNotExist()
    {
        // Arrange
        var userId = 1;
        _profileServiceMock.Setup(service => service.GetProfileByUserId(userId)).ThrowsAsync(new KeyNotFoundException("Profile not found"));

        // Act
        var result = await _profileController.GetProfileByUserId(userId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task AddProfile_ReturnsCreated_WhenProfileIsAdded()
    {
        // Arrange
        var profileDto = new ProfileDto { ProfileId = 1 };
        _profileServiceMock.Setup(service => service.AddProfile(profileDto)).ReturnsAsync(profileDto);

        // Act
        var result = await _profileController.AddProfile(profileDto) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status201Created, result.StatusCode);
        ClassicAssert.AreEqual(profileDto, result.Value);
    }

    [Test]
    public async Task AddProfile_ReturnsBadRequest_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        var profileDto = new ProfileDto { ProfileId = 1 };
        _profileServiceMock.Setup(service => service.AddProfile(profileDto)).ThrowsAsync(new DbUpdateException("Database error"));

        // Act
        var result = await _profileController.AddProfile(profileDto) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Test]
    public async Task UpdateProfile_ReturnsOk_WhenProfileIsUpdated()
    {
        // Arrange
        var profileDto = new ProfileDto { ProfileId = 1 };
        _profileServiceMock.Setup(service => service.UpdateProfile(profileDto)).ReturnsAsync(profileDto);

        // Act
        var result = await _profileController.UpdateProfile(profileDto) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profileDto, result.Value);
    }

    [Test]
    public async Task UpdateProfile_ReturnsNotFound_WhenProfileDoesNotExist()
    {
        // Arrange
        var profileDto = new ProfileDto { ProfileId = 1 };
        _profileServiceMock.Setup(service => service.UpdateProfile(profileDto)).ThrowsAsync(new KeyNotFoundException("Profile not found"));

        // Act
        var result = await _profileController.UpdateProfile(profileDto) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task DeleteProfileById_ReturnsOk_WhenProfileIsDeleted()
    {
        // Arrange
        var profileId = 1;
        var profileDto = new ProfileDto { ProfileId = profileId };
        _profileServiceMock.Setup(service => service.DeleteProfileById(profileId)).ReturnsAsync(profileDto);

        // Act
        var result = await _profileController.DeleteProfileById(profileId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profileDto, result.Value);
    }

    [Test]
    public async Task DeleteProfileById_ReturnsNotFound_WhenProfileDoesNotExist()
    {
        // Arrange
        var profileId = 1;
        _profileServiceMock.Setup(service => service.DeleteProfileById(profileId)).ThrowsAsync(new KeyNotFoundException("Profile not found"));

        // Act
        var result = await _profileController.DeleteProfileById(profileId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetMatches_ReturnsOk_WhenMatchesExist()
    {
        // Arrange
        var profileId = 1;
        var matches = new List<ProfilePreviewDto> { new ProfilePreviewDto { ProfileId = 1 } };
        _profileServiceMock.Setup(service => service.GetProfileMatches(profileId)).ReturnsAsync(matches);

        // Act
        var result = await _profileController.GetMatches(profileId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(matches, result.Value);
    }

    [Test]
    public async Task GetMatches_ReturnsNotFound_WhenNoMatchesExist()
    {
        // Arrange
        var profileId = 1;
        _profileServiceMock.Setup(service => service.GetProfileMatches(profileId)).ThrowsAsync(new KeyNotFoundException("No matches found"));

        // Act
        var result = await _profileController.GetMatches(profileId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetAll_ReturnsOk_WhenProfilesExist()
    {
        // Arrange
        var profiles = new List<ProfileDto> { new ProfileDto { ProfileId = 1 } };
        _profileServiceMock.Setup(service => service.GetAll()).ReturnsAsync(profiles);

        // Act
        var result = await _profileController.GetAll() as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profiles, result.Value);
    }
}