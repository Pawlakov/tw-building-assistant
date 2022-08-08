namespace TWBuildingAssistant.Domain;

using TWBuildingAssistant.Domain.Exceptions;

public static class SeasonOperations
{
    public static Season Create(in int id, in string name)
    {
        return (id, name) switch
        {
            (0, _) =>
                throw new DomainRuleViolationException("Season without id."),
            (_, null or "") =>
                throw new DomainRuleViolationException("Season without name."),
            _ =>
                new Season(id, name),
        };
    }
}