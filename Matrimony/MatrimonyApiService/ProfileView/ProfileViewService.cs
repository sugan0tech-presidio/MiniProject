﻿using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Profile;
using MatrimonyApiService.User;

namespace MatrimonyApiService.ProfileView;

using System;
using System.Threading.Tasks;

public class ProfileViewService(
    IBaseRepo<ProfileView> profileViewRepo,
    IMembershipService membershipService,
    IUserService userService,
    IProfileService profileService,
    IMapper mapper,
    ILogger<ProfileViewService> logger) : IProfileViewService
{
    /// <inheritdoc/>
    public async Task AddView(int viewerId, int profileId)
    {
        if (viewerId.Equals(profileId))
            throw new InvalidProfileViewException("You are looking into your own managed profile bruh!!");
        await profileService.GetProfileById(profileId); // validate profile
        await profileService.GetProfileById(viewerId); // validate profile
        var profile = await profileService.GetProfileById(viewerId);
        var membership = await membershipService.GetByProfileId(profile.ProfileId);
        if (!membership.IsTrail)
        {
            if (membership.Type.Equals(MemberShip.FreeUser.ToString()))
                throw new NonPremiumUserException(
                    "You can't View a profile under free account\n consider upgrading your membership.");
            if (membership.Type.Equals(MemberShip.BasicUser.ToString()) && membership.ViewsCount >= 50)
                throw new ExhaustedMaximumProfileViewsException(
                    "You have reached limit of viewing 50 profiles, Want to see more profiles consider upgrading");
        }

        var profileView = new ProfileView { ViewedProfileAt = profileId, ViewerId = viewerId, ViewedAt = DateTime.Now };
        await profileViewRepo.Add(profileView);
        membership.ViewsCount++;
        await membershipService.Update(membership);
    }

    /// <inheritdoc/>
    public async Task AddView(ProfileViewDto profileViewDto)
    {
        var profileView = mapper.Map<ProfileView>(profileViewDto);
        await profileViewRepo.Add(profileView);
    }

    /// <inheritdoc/>
    public async Task<ProfileViewDto> GetViewById(int viewId)
    {
        try
        {
            var profileViewEntity = await profileViewRepo.GetById(viewId);
            return mapper.Map<ProfileViewDto>(profileViewEntity);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<List<ProfileViewDto>> GetViewsByProfileId(int profileId)
    {
        var allViews = await profileViewRepo.GetAll();
        try
        {
            var membership = await membershipService.GetByProfileId(profileId);
            var views = allViews.FindAll(view => view.ViewedProfileAt.Equals(profileId))
                .ConvertAll(mapper.Map<ProfileViewDto>).ToList();
            if (membership == null || membership.Type.Equals(MemberShip.FreeUser.ToString()))
                throw new NonPremiumUserException("At least you have to be a basic user to see who watched your profile ");
            if (membership.Type.Equals(MemberShip.BasicUser.ToString()))
            {
                var filteredViews = views
                    .Where(view => view.ViewedProfileAt == profileId && view.ViewedAt > DateTime.Now.AddMonths(-1))
                    .OrderBy(view => view.ViewedAt)
                    .Take(5)
                    .ToList();
                return filteredViews;
            }

            return views;
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError($"Profile with {profileId} not found" + e.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteViewById(int viewId)
    {
        try
        {
            await profileViewRepo.DeleteById(viewId);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteOldViews(DateTime before)
    {
        if (DateTime.Now < before)
        {
            logger.LogError($"{before} is in near feature, Cleanup dates should be at-least a day older");
            throw new InvalidDateTimeException(
                $"{before} is in near feature, Cleanup dates should be at-least a day older");
        }

        var views = await profileViewRepo.GetAll();
        var viewsToDelete = views.Where(view => view.ViewedAt.CompareTo(before) < 0).ToList();
        foreach (var viewToDelete in viewsToDelete) await profileViewRepo.DeleteById(viewToDelete.Id);
    }
}