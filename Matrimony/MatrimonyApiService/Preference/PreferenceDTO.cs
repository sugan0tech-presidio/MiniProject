﻿using MatrimonyApiService.Profile;

namespace MatrimonyApiService.Preference;

public record PreferenceDto
{
    public int PreferenceId { get; init; }
    public string Gender { get; set; }
    public string MotherTongue { get; init; }
    public string Religion { get; init; }
    public string Education { get; init; }
    public string Occupation { get; init; }
    public int MinHeight { get; init; }
    public int MaxHeight { get; init; }
    public int MinAge { get; init; }
    public int MaxAge { get; init; }
    public int PreferenceForId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}