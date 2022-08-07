namespace TWBuildingAssistant.Domain;

using TWBuildingAssistant.Domain.Exceptions;

public static class WeatherOperations
{
    public static Weather Create(in string name)
    {
        return name switch
        {
            null or "" =>
                throw new DomainRuleViolationException("Weather without name."),
            _ =>
                new Weather(name),
        };
    }
}