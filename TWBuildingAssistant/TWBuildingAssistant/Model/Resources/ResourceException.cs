namespace TWBuildingAssistant.Model.Resources
{
    using System;

    public class ResourceException : Exception
    {
        public ResourceException() 
        : base("Failure concerning resources.")
        {
        }

        public ResourceException(string message) 
        : base(message)
        {
        }

        public ResourceException(string message, Exception innerException) 
        : base(message, innerException)
        {
        }
    }
}