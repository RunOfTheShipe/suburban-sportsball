namespace Sportsball.Core.Tests.Utils;

internal class LeagueSettingsBuilder
{
    private readonly LeagueSettings _settings;

    public LeagueSettingsBuilder()
    {
        _settings = new LeagueSettings();
    }

    public LeagueSettings Build()
    {
        return _settings;
    }

    public LeagueSettingsBuilder AddMetric(string metric, decimal value)
    {
        _settings.Metrics[metric] = value;
        return this;
    }

    public LeagueSettingsBuilder AddPosition(string position, int count = 1)
    {
        _settings.PositionsMap[position] = count;
        return this;
    }
}
