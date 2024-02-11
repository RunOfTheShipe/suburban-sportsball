namespace Sportsball.Core;

/// <summary>
/// Represents the stats for a player in a game
/// </summary>
public readonly struct PlayerStats
{
    public PlayerStats(PlayerID playerID, string position)
    {
        ID = playerID;
        Position = position;
    }

    /// <summary>
    /// ID of the player the stats are associated with
    /// </summary>
    public PlayerID ID { get; init; }

    /// <summary>
    /// The position the player is listed as on a roster
    /// </summary>
    public string Position { get; init; }

    /// <summary>
    /// A player's stats for a single game; maps the statistic to a value
    /// </summary>
    public Dictionary<string, decimal> Stats { get; init; } = new Dictionary<string, decimal>();
}

public record PlayerID(Guid ID);
