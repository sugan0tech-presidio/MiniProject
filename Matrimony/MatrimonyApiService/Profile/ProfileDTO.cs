﻿using MatrimonyApiService.Match;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Preference;
using MatrimonyApiService.ProfileView;
using MatrimonyApiService.User;
using MatrimonyApiService.Validations;

namespace MatrimonyApiService.Profile;

public record ProfileDto
{
    public int ProfileId { get; init; }
    public DateTime DateOfBirth { get; init; }
    public int Age { get; init; }

    [EnumTypeValidation(typeof(Enums.Education))]
    public string Education { get; init; }

    public int AnnualIncome { get; init; }

    [EnumTypeValidation(typeof(Enums.Occupation))]
    public string Occupation { get; init; }

    [EnumTypeValidation(typeof(Enums.MaritalStatus))]
    public string MaritalStatus { get; init; }

    [EnumTypeValidation(typeof(Enums.MotherTongue))]
    public string MotherTongue { get; init; }

    [EnumTypeValidation(typeof(Enums.Religion))]
    public string Religion { get; init; }

    [EnumTypeValidation(typeof(Enums.Ethnicity))]
    public string Ethnicity { get; init; }

    public string? Bio { get; init; }
    public byte[]? ProfilePicture { get; init; }
    public bool Habits { get; init; }

    [EnumTypeValidation(typeof(Enums.Gender))]
    public string Gender { get; init; }

    public int Weight { get; init; }
    public int Height { get; init; }
    public int MembershipId { get; init; }
    public MembershipDto? Membership { get; init; }
    public int ManagedById { get; init; }
    public UserDto? ManagedBy { get; init; }
    public int UserId { get; init; }
    public UserDto? User { get; init; }
    public string ManagedByRelation { get; init; }
    public IEnumerable<ProfileViewDto>? ProfileViews { get; init; }
    public IEnumerable<MatchDto>? SentMatches { get; init; }
    public IEnumerable<MatchDto>? ReceivedMatches { get; init; }
    public int PreferenceId { get; init; }
    public PreferenceDto? Preference { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}