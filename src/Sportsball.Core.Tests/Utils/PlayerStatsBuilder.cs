using System;

namespace Sportsball.Core.Tests.Utils;

internal class PlayerStatsBuilder
{
    private readonly PlayerStats _stats;

    public PlayerStatsBuilder(string position = "")
    {
        _stats = new PlayerStats(new PlayerID(Guid.NewGuid()), position);
    }

    public PlayerStatsBuilder Add(string stat, decimal value)
    {
        _stats.Stats[stat] = value;
        return this;
    }

    public PlayerStats Build()
    {
        return _stats;
    }
}
