using System;
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
            .AddMetric("RushYards", 1/10M)
            .AddMetric("RecYards", 1/20M)
            .AddMetric("PassYards", 1/20M)
            .AddMetric("TD", 6)
            .Build();

        var playerStats = new PlayerStatsBuilder()
            .Add("RushYards", 20)
            .Add("RecYards", 50)
            .Build();

        new Scoring(settings).ScorePlayer(playerStats)
            .Should().Be(20*1/10M + 50*1/20M);
    }

    [Fact]
    public void ScorePlayer_WithNegativeAndZero()
    {
        var settings = new LeagueSettingsBuilder()
            .AddMetric("RushYards", 1/10M)
            .AddMetric("RecYards", 1/20M)
            .AddMetric("PassYards", 1/20M)
            .AddMetric("TD", 6)
            .Build();

        var playerStats = new PlayerStatsBuilder()
            .Add("RushYards", -10)
            .Add("RecYards", 0)
            .Build();

        new Scoring(settings).ScorePlayer(playerStats)
            .Should().Be(-10 * 1/10M);
    }
}
