namespace Sportsball.Core.Tests.Utils;

public class LineupBuilder
{
    private readonly Lineup _lineup;

    public LineupBuilder()
    {
        _lineup = new Lineup();
    }

    public Lineup Build()
    {
        return _lineup;
    }

    public LineupBuilder AddPlayer(PlayerID pid, string position)
    {
        _lineup.PlayerPositions[pid] = position;
        return this;
    }
}
