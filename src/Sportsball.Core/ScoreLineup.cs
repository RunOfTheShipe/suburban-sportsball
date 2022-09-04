namespace Sportsball.Core;

public class ScoreLineup
{
    private readonly Lineup _lineup;
    private readonly IStats _stats;
    private readonly ILeagueSettings _settings;

    public ScoreLineup(Lineup lineup, IStats stats, ILeagueSettings settings)
    {
        _lineup = lineup ?? throw new ArgumentNullException(nameof(lineup));
        _stats = stats ?? throw new ArgumentNullException(nameof(stats));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public Decimal Calculate()
    {
        // calculate the score for each player in each line up
        var scores = new List<PlayerScore>();
        foreach (var player in _lineup.Players.Keys)
        {
            // grab the stats for the current player
            var playerStats = _stats.GetStats(player);

            // enumerate the stats the league is interested in calculating
            decimal playerPts = 0;
            foreach (var statCfg in _settings.Scoring)
            {
                // get the player's stat - if that stat isn't recorded, skip it
                if (playerStats.Stats.TryGetValue(statCfg.Name, out decimal stat))
                {
                    playerPts += stat * statCfg.Value;
                }
            }

            var score = new PlayerScore(player, _lineup.Players[player], playerPts);
            scores.Add(score);
        }

        // TODO@BMS: FLEX positions

        // group by position, take the highest score of the position, sum all the scores
        return scores.GroupBy(ps => ps.Position.Name)
            .Select(position => position.Max(ps => ps.Score))
            .Sum();
    }

    private class PlayerScore
    {
        public PlayerScore(Player player, Position position, decimal score)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            Position = position ?? throw new ArgumentNullException(nameof(position));
            Score = score;
        }

        public Player Player { get; init; }
        public Position Position { get; init; }
        public Decimal Score { get; init; }
    }
}



public readonly record struct StatConfig(string DisplayName, string Name, Decimal Value);
public readonly record struct StatValue(string Name, Decimal Value);

public interface IStats
{
    IPlayerStats GetStats(Player player);
}

public interface IPlayerStats
{
    string Player { get; }
    Dictionary<string, Decimal> Stats { get; }
}

public interface ILeagueSettings
{
    List<StatConfig> Scoring { get; }
}



public class Lineup
{
    public Dictionary<Player, Position> Players { get; set; } = new Dictionary<Player, Position>();
}

public class Position
{
    public Position(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        Name = name;
    }

    public string Name { get; init; }
}

public class Player
{
    public Player(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        Name = name;
    }

    public string Name { get; init; }
}
