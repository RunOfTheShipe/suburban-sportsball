namespace Sportsball.Core;

public readonly struct Lineup
{
    public Lineup()
    { }

    /// <summary>
    /// Represents a map between the player and the position the player is in the lineup
    /// </summary>
    public Dictionary<PlayerID, string> PlayerPositions { get; init; } = new Dictionary<PlayerID, string>();
}
