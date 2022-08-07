namespace TWBuildingAssistant.Domain;

using TWBuildingAssistant.Domain.Exceptions;

public static class SeasonOperations
{
    public static Season Create(in string name)
    {
        return name switch
        {
            null or "" =>
                throw new DomainRuleViolationException("Season without name."),
            _ =>
                new Season(name),
        };
    }
}