namespace TWBuildingAssistant.Model.Resources
{
    using System;

    public class ResourcesException : Exception
    {
        public ResourcesException() 
        : base("Failure concerning resources.")
        {
        }

        public ResourcesException(string message) 
        : base(message)
        {
        }

        public ResourcesException(string message, Exception innerException) 
        : base(message, innerException)
        {
        }
    }
}