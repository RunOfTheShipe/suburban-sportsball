using System.ComponentModel;

namespace Sportsball.Core;

public class Scoring
{
    private readonly LeagueSettings _settings;

    public Scoring(LeagueSettings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Calculates the score for the given player, based on the league settings
    /// </summary>
    /// <param name="player">Player to score</param>
    /// <returns>Score for the player</returns>
    public decimal ScorePlayer(PlayerStats player)
    {
        decimal score = 0;

        foreach (var stat in player.Stats)
        {
            if (_settings.Metrics.TryGetValue(stat.Key, out decimal normalizedValue))
            {
                score += stat.Value * normalizedValue;
            }
        }

        return score;
    }

    /// <summary>
    /// Calculates the score for the full lineup. Takes into account the position each
    /// player was assigned in the line-up and how many of those players are allowed in
    /// each position.
    /// </summary>
    /// <returns>Score for the lineup</returns>
    public decimal ScoreLineup(Lineup lineup, Dictionary<PlayerID, PlayerStats> players)
    {
        // Calculate the score for each player
        var positionPoints = new Dictionary<string, List<decimal>>();
        foreach (var kvp in lineup.PlayerPositions)
        {
            var playerID = kvp.Key;
            string position = kvp.Value;

            if (players.TryGetValue(playerID, out var playerStats))
            {
                var playerScore = ScorePlayer(playerStats);

                // Add the player's points to the associated position
                if (!positionPoints.ContainsKey(position))
                {
                    positionPoints[position] = new List<decimal>();
                }
                positionPoints[position].Add(playerScore);
            }
        }

        // for each position, go through and take the highest scores with the configured settings
        decimal totalScore = 0;
        foreach (var pp in positionPoints)
        {
            string position = pp.Key;
            List<decimal> scores = pp.Value;

            var positionScore = scores.OrderDescending()
                .Take(_settings.PositionsMap[position])
                .Sum();

            totalScore += positionScore;
        }

        // For each position, take the N highest scores associated with the position
        return totalScore;
    }
}
