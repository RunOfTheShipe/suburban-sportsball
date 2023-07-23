
namespace Sportsball.Core;

/// <summary>
/// Represents the stats for a player in a game
/// </summary>
public readonly struct PlayerStats
{
    public PlayerStats()
    { }

    /// <summary>
    /// ID of the player
    /// </summary>
    public Guid PlayerID { get; init; }

    /// <summary>
    /// A player's stats for a single game; maps the statistic to a value
    /// </summary>
    public Dictionary<string, decimal> Stats { get; init; } = new Dictionary<string, decimal>();
}
