using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Sportsball.Core;
using FluentAssertions;

namespace Sportsball.Core.Tests;

public class ScoringTests
{
    [Fact]
    public void BasicGameScore()
    {
        // GIVEN:   The following line up
        var lineup = new Lineup()
        {
            Players = new Dictionary<Player, Position>()
            {
                { new Player("QB1"), new Position("QB") },
                { new Player("QB2"), new Position("QB") },
                { new Player("WR1"), new Position("WR") },
                { new Player("WR2"), new Position("WR") },
                { new Player("RB1"), new Position("RB") }
            }
        };

        // AND:     The QBs have the following stats
        var qb1Stats = new PlayerStats("QB1",
            new StatValue("PTD", 2),
            new StatValue("PY", 100),
            new StatValue("INT", 0));

        var qb2Stats = new PlayerStats("QB2",
            new StatValue("PTD", 0),
            new StatValue("PY", 100),
            new StatValue("INT", 0));

        // AND:     The WRs have the same total points
        var wr1Stats = new PlayerStats("WR1",
            new StatValue("RY", 10),
            new StatValue("REY", 20));

        var wr2Stats = new PlayerStats("WR2",
            new StatValue("RY", 20),
            new StatValue("REY", 10));

        // AND:     The RB has negative stats
        var rb1Stats = new PlayerStats("RB1",
            new StatValue("RY", -10));

        // WHEN:    The lineup is scored
        var game = new ScoreLineup(lineup,
            new Stats(qb1Stats, qb2Stats, wr1Stats, wr2Stats, rb1Stats),
            new LeagueSettings());

        // THEN:    The score should be
        //              (QB=12, WR=3, RB=-1)
        game.Calculate().Should().Be(14);
    }

    internal class Stats : IStats
    {
        private readonly Dictionary<string, IPlayerStats> _playerStats;

        public Stats(params PlayerStats[] playerStats)
        {
            _playerStats = new Dictionary<string, IPlayerStats>();
            foreach (var ps in playerStats)
            {
                _playerStats.Add(ps.Player, ps);
            }
        }

        public IPlayerStats GetStats(Player player)
        {
            if (_playerStats.ContainsKey(player.Name))
            {
                return _playerStats[player.Name];
            }
            return new PlayerStats(player.Name);
        }
    }

    internal class PlayerStats : IPlayerStats
    {
        private readonly string _name;
        private readonly Dictionary<string, Decimal> _stats;

        public PlayerStats(string name, params StatValue[] stats)
        {
            _name = name;

            _stats = new Dictionary<string, decimal>();
            foreach (var stat in stats)
            {
                _stats.Add(stat.Name, stat.Value);
            }
        }

        public string Player { get { return _name; } }

        public Dictionary<string, decimal> Stats { get { return _stats; } }
    }

    internal class LeagueSettings : ILeagueSettings
    {
        private readonly List<StatConfig> _scoring = new List<StatConfig>()
        {
            new StatConfig("Passing Yards", "PY", 0.04m),
            new StatConfig("TD Pass", "PTD", 4m),
            new StatConfig("Interceptions Thrown", "INT", -2m),
            new StatConfig("2pt Passing Conversion", "2PC", 2m),
            new StatConfig("Rushing Yards", "RY", 0.1m),
            new StatConfig("TD Rush", "RTD", 6m),
            new StatConfig("2pt Rushing Conversion", "2PR", 2m),
            new StatConfig("Receiving Yards", "REY", 0.1m),
            new StatConfig("Each reception", "REC", 0.5m),
            new StatConfig("TD Reception", "RETD", 6m),
            new StatConfig("2pt Receiving Conversion", "2PRE", 2m),
            new StatConfig("Team Win", "TW", 5m),
            new StatConfig("Team Loss", "TL", -1m),
            new StatConfig("Kickoff Return TD", "KRTD", 6m),
            new StatConfig("Punt Return TD", "PRTD", 6m),
            new StatConfig("Fumble Recovered for TD", "FTD", 6m),
            new StatConfig("Total Fumbles Lost", "FUML", -2m),
            new StatConfig("Interception Return TD", "INTTD", 6m),
            new StatConfig("Fumble Return TD", "FRTD", 6m),
            new StatConfig("Blocked Punt or FG return for TD", "BLKKRTD", 6m),
            new StatConfig("2pt Return", "2PTRET", 2m),
            new StatConfig("1pt Safety", "1PSF", 1m),
        };

        public List<StatConfig> Scoring { get { return _scoring; } }
    }
}
