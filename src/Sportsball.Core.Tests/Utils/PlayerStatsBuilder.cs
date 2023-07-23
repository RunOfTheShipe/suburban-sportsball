using System;

namespace Sportsball.Core.Tests.Utils;

internal class PlayerStatsBuilder
{
    private readonly PlayerStats _stats;

    public PlayerStatsBuilder()
    {
        _stats = new PlayerStats(new PlayerID(Guid.NewGuid()));
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
