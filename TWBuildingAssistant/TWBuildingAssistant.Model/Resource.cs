﻿namespace TWBuildingAssistant.Model
{
    public class Resource
    {
        public Resource(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainRuleViolationException("Resource without name.");
            }

            this.Name = name;
        }

        public string Name { get; }
    }
}