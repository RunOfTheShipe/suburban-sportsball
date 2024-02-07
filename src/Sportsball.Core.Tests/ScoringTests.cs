using System;
using System.Collections.Generic;
using System.ComponentModel;
using FluentAssertions;
using Sportsball.Core;
using Sportsball.Core.Tests.Utils;
using Xunit;

namespace Sportsball.Core.Tests;

public class ScoringTests
{
    [Fact]
    public void ScorePlayer_Basic()
    {
        var settings = new LeagueSettingsBuilder()
            .AddMetric("RushYards", 1 / 10M)
            .AddMetric("RecYards", 1 / 20M)
            .AddMetric("PassYards", 1 / 20M)
            .AddMetric("TD", 6)
            .Build();

        var playerStats = new PlayerStatsBuilder()
            .Add("RushYards", 20)
            .Add("RecYards", 50)
            .Build();

        new Scoring(settings).ScorePlayer(playerStats)
            .Should().Be(20 * 1 / 10M + 50 * 1 / 20M);
    }

    [Fact]
    public void ScorePlayer_WithNegativeAndZero()
    {
        var settings = new LeagueSettingsBuilder()
            .AddMetric("RushYards", 1 / 10M)
            .AddMetric("RecYards", 1 / 20M)
            .AddMetric("PassYards", 1 / 20M)
            .AddMetric("TD", 6)
            .Build();

        var playerStats = new PlayerStatsBuilder()
            .Add("RushYards", -10)
            .Add("RecYards", 0)
            .Build();

        new Scoring(settings).ScorePlayer(playerStats)
            .Should().Be(-10 * 1 / 10M);
    }

    [Fact]
    public void ScoreLineup_BestPlayerForPosition()
    {
        // GIVEN:   a lineup with 2 RB positions
        var settings = new LeagueSettingsBuilder()
            .AddMetric("RushYards", 1 / 10M)
            .AddMetric("RecYards", 1 / 20M)
            .AddMetric("PassYards", 1 / 20M)
            .AddMetric("TD", 6)
            .AddPosition("RB", 2)
            .Build();

        // AND:     Three running backs
        var rb1 = new PlayerStatsBuilder()
            .Add("RushYards", 100)  // 5pts
            .Add("TD", 1)           // 6pts
            .Build();

        var rb2 = new PlayerStatsBuilder()
            .Add("RushYards", 100)  // 5pts
            .Add("TD", 2)           // 12pts
            .Build();

        var rb3 = new PlayerStatsBuilder()
            .Add("RushYards", 100)  // 5pts
            .Add("TD", 3)           // 18pts
            .Build();

        var players = new Dictionary<PlayerID, PlayerStats>()
        {
            { rb1.ID, rb1 },
            { rb2.ID, rb2 },
            { rb3.ID, rb3 },
        };

        var lineup = new LineupBuilder()
            .AddPlayer(rb1.ID, "RB")
            .AddPlayer(rb2.ID, "RB")
            .AddPlayer(rb3.ID, "RB")
            .Build();

        // WHEN:    The lineup is scored
        var scoring = new Scoring(settings);
        var actualPts = scoring.ScoreLineup(lineup, players);

        // THEN:    The best two running backs should be selected
        actualPts.Should().Be(40);  // 17 (rb1) + 23 (rb2)
    }

    [Fact]
    public void ScoreLineup_Basic()
    {
        var settings = new LeagueSettingsBuilder()
            .AddMetric("RushYards", 1 / 10M)
            .AddMetric("RecYards", 1 / 20M)
            .AddMetric("PassYards", 1 / 20M)
            .AddMetric("TD", 6)
            .AddPosition("QB", 1)
            .AddPosition("RB", 2)
            .AddPosition("WR", 3)
            .Build();

        var qb1 = new PlayerStatsBuilder()
            .Add("PassYards", 100)  // 5pts
            .Add("TD", 1)           // 6pts
            .Build();

        var qb2 = new PlayerStatsBuilder()
            .Add("PassYards", 200)  // 10pts
            .Build();

        var rb1 = new PlayerStatsBuilder()
            .Add("RushYards", 50)   // 5pts
            .Add("TD", 2)           // 12pts
            .Build();

        var rb2 = new PlayerStatsBuilder()
            .Add("RushYards", 100)  // 10pts
            .Build();

        var players = new Dictionary<PlayerID, PlayerStats>()
        {
            { qb1.ID, qb1 },
            { qb2.ID, qb2 },
            { rb1.ID, rb1 },
            { rb2.ID, rb2 },
        };

        var lineup = new LineupBuilder()
            .AddPlayer(qb1.ID, "QB")
            .AddPlayer(qb2.ID, "QB")
            .AddPlayer(rb1.ID, "RB")
            .AddPlayer(rb2.ID, "RB")
            .Build();

        // score the line-up
        var scoring = new Scoring(settings);
        var actualPts = scoring.ScoreLineup(lineup, players);

        // confirm the points match
        //  - QB: QB1 should be selected (11 pts vs 10 pts)
        //  - RB: Both running backs included (17 pts + 10 pts)
        //  - WR: None included since none were listed in the lineup
        actualPts.Should().Be(38);
    }
}
