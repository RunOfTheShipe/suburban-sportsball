namespace Sportsball.Core.Tests;

using System.Collections.Generic;
using FluentAssertions;
using Sportsball.Core;
using Sportsball.Core.Tests.Utils;
using Xunit;
using Xunit.Sdk;

public class LineupBuilderTests
{
    [Fact]
    public void TestIt()
    {
        // GIVEN:   the league settings
        var settings = new LeagueSettingsBuilder()
            .AddMetric("RushYards", 1 / 10M)
            .AddMetric("RecYards", 1 / 20M)
            .AddMetric("PassYards", 1 / 20M)
            .AddMetric("TD", 6)
            .AddPosition("QB", 1)
            .AddPosition("RB", 1)
            .AddPosition("WR", 1)
            .AddPosition("FLEX", 2)
            .Build();

        // AND:     some players
        var qb1 = new PlayerStatsBuilder("QB")
            .Add("PassYards", 100)  // 5pts
            .Add("TD", 1)           // 6pts
            .Build();
        var qb2 = new PlayerStatsBuilder("QB")
            .Add("PassYards", 200)  // 10pts
            .Build();

        var rb1 = new PlayerStatsBuilder("RB")
            .Add("RushYards", 50)   // 5pts
            .Add("TD", 2)           // 12pts
            .Build();
        var rb2 = new PlayerStatsBuilder("RB")
            .Add("RushYards", 100)  // 10pts
            .Build();

        var wr1 = new PlayerStatsBuilder("WR")
            .Add("RecYards", 100)   // 5pts
            .Build();
        var wr2 = new PlayerStatsBuilder("WR")
            .Add("RecYards", 160)   // 8pts
            .Build();

        // WHEN:    the lineup is built
        var actual = new Core.LineupBuilder(settings)
            .Create(new List<PlayerStats>() { qb1, qb2, rb1, rb2, wr1, wr2 });

        // THEN:    it should be
        var expected = new Utils.LineupBuilder()
            .AddPlayer(qb1.ID, "QB")
            .AddPlayer(rb1.ID, "RB")
            .AddPlayer(wr1.ID, "WR")
            .AddPlayer(rb2.ID, "FLEX")
            .AddPlayer(wr2.ID, "FLEX")
            .Build();
        
        actual.PlayerPositions.Should().BeEquivalentTo(expected.PlayerPositions);
    }
}
