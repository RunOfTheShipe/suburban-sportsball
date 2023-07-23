namespace Sportsball.Core;

/// <summary>
/// Represents the settings for a league
/// </summary>
public readonly struct LeagueSettings
{
    public LeagueSettings()
    { }

    /// <summary>
    /// Represents the positions in a line-up - both the positions and the number of playes
    /// at each position
    /// </summary>
    public Dictionary<string, int> PositionsMap { get; init; } = new Dictionary<string, int>();

    /// <summary>
    /// Represents a map of "metric to points" (e.g.: yards / pt). Metrics represent a normalized
    /// value
    /// </summary>
    public Dictionary<string, decimal> Metrics { get; init; } = new Dictionary<string, decimal>();
}
