namespace TWBuildingAssistant.Domain;

using TWBuildingAssistant.Domain.Exceptions;

public static class WeatherOperations
{
    public static Weather Create(in int id, in string name)
    {
        return (id, name) switch
        {
            (0, _) =>
                throw new DomainRuleViolationException("Weather without id."),
            (_, null or "") =>
                throw new DomainRuleViolationException("Weather without name."),
            _ =>
                new Weather(id, name),
        };
    }
}