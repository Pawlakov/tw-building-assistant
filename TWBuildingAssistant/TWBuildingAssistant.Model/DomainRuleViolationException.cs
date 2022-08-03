namespace TWBuildingAssistant.Model;

using System;

public class DomainRuleViolationException : Exception
{
    public DomainRuleViolationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DomainRuleViolationException(string message)
        : base(message)
    {
    }

    public DomainRuleViolationException()
        : base("One of the domain rules was violated.")
    {
    }
}