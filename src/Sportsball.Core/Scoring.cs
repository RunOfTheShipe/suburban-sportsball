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
}
