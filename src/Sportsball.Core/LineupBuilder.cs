using System.Data.Common;

namespace Sportsball.Core;

public class LineupBuilder
{
    private readonly LeagueSettings _settings;

    public LineupBuilder(LeagueSettings settings)
    {
        _settings = settings;
    }

    public Lineup Create(List<PlayerStats> players)
    {
        // start with an empty lineup and add the first player
        // call recursively minus the first player
        var working = new WorkingLineup();

        // create the line up based on the league settings, but leave all the players empty
        foreach (var (pos, count) in _settings.PositionsMap)
        {
            working.Positions[pos] = new PlayerStats?[count];
        }

        var best = Recurse(players, working);
        
        var lineup = new Lineup();
        foreach (var (pos, positionPlayers) in best.Positions)
        {
            foreach (var pp in positionPlayers)
            {
                if (pp.HasValue)
                {
                    lineup.PlayerPositions[pp.Value.ID] = pos;
                }
            }
        }
        return lineup;
    }

    private WorkingLineup Recurse(List<PlayerStats> players, WorkingLineup current)
    {
        // breaking condition - the lineup is already full or there aren't any available
        // players to add to the lineup, there isn't anything to do
        if (IsFull(ref current) || players.Count == 0)
        {
            return current;
        }

        // at this point, the lineup isn't full, so we need to try to add each of the
        // players to each of the positions, calculate the score of each lineup, and
        // remember which one was the best

        WorkingLineup best = new WorkingLineup();
        decimal bestScore = 0;

        // try to add each player in each open position
        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];
            var remainingPlayers = players.Where(p => p.ID != player.ID).ToList();


            // put the player in the first open position and then recurse
            foreach (var (pos, positionPlayers) in current.Positions)
            {
                // for the current position, look to see if there is an opening the player
                // can fit into
                for (int j = 0; j < positionPlayers.Length; j++)
                {
                    // TODO@BMS: also, check to see if the player can play in the position
                    if (positionPlayers[j] == null)
                    {
                        positionPlayers[j] = player;

                        // recurse to fill the rest of the lineup - eventually, we'll have a full
                        // lineup
                        // NOTE: the lineup may not be full, but due to the set of players and
                        // league settings, it may not be possible to create a full lineup
                        var full = Recurse(players, current);
                        
                        // get the score of the current lineup
                        var currentScore = Score(ref full, _settings);

                        // compare to the current "best" score
                        if (currentScore > bestScore)
                        {
                            // best lineup we've seen yet, save a copy of it
                            best = full.Copy();
                            bestScore = currentScore;
                        }

                        // done with adding this player in this position - reset the position to null
                        // and try adding the player at the next position
                        // NOTE: Order of a player in the position is not important - can break out
                        // of this position loop and move to the next position
                        positionPlayers[j] = null;
                        break;       
                    }
                }
            }
        }

        return best;
    }

    /// <summary>
    /// Internal helper to better help with dynamically building a lineup
    /// </summary>
    private struct WorkingLineup
    {
        /// <summary>
        /// A map of the name of the position to the set of players associated with the position
        /// </summary>
        public Dictionary<string, PlayerStats?[]> Positions;

        public WorkingLineup()
        {
            Positions = new Dictionary<string, PlayerStats?[]>();
        }

        /// <summary>
        /// Creates a copy of the lineup
        /// </summary>
        /// <returns>WorkingLineup</returns>
        public WorkingLineup Copy()
        {
            var copy = new WorkingLineup();

            foreach (var (pos, players) in Positions)
            {
                var array = new PlayerStats?[players.Length];
                for (int i = 0; i < players.Length; i++)
                {
                    array[i] = players[i];
                }
                copy.Positions[pos] = array;
            }

            return copy;
        }
    }

    /// <summary>
    /// Checks to see if all positions in the lineup have been filled
    /// </summary>
    /// <param name="lineup">Lineup to check</param>
    /// <returns>True if the lineup is full</returns>
    private bool IsFull(ref WorkingLineup lineup)
    {
        foreach (var kvp in lineup.Positions)
        {
            for (int i = 0; i < kvp.Value.Length; i++)
            {
                if (kvp.Value[i] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Score calculates the score for the given lineup by scoring each player; any
    /// null positions are ignored.
    /// </summary>
    /// <param name="lineup">Lineup to score</param>
    /// <param name="settings">Settings for the league</param>
    /// <returns>Score of the lineup</returns>
    private decimal Score(ref WorkingLineup lineup, LeagueSettings settings)
    {
        decimal score = 0;
        var scoring = new Scoring(settings);

        foreach (var kvp in lineup.Positions)
        {
            for (int i = 0; i < kvp.Value.Length; i++)
            {
                var player = kvp.Value[i];
                if (player.HasValue)
                {
                    score += scoring.ScorePlayer(player.Value);
                }
            }
        }

        return score;
    }
}
