namespace TWBuildingAssistant.Domain;

using TWBuildingAssistant.Domain.Exceptions;

public static class ResourceOperations
{
    public static Resource Create(in int id, in string name)
    {
        return (id, name) switch
        {
            (0, _) =>
                throw new DomainRuleViolationException("Resource without id."),
            (_, null or "") =>
                throw new DomainRuleViolationException("Resource without name."),
            _ =>
                new Resource(id, name),
        };
    }
}