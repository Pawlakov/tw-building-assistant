namespace TWBuildingAssistant.Domain;

using TWBuildingAssistant.Domain.Exceptions;

public static class ResourceOperations
{
    public static Resource Create(in string name)
    {
        return name switch
        {
            null or "" =>
                throw new DomainRuleViolationException("Resource without name."),
            _ =>
                new Resource(name),
        };
    }
}