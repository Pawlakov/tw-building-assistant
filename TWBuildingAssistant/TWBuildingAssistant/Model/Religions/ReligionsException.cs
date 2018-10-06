namespace TWBuildingAssistant.Model.Religions
{
    using System;
    
    public class ReligionsException : Exception
    {
        public ReligionsException()
        : base("Failure concerning religions.")
        {
        }
        
        public ReligionsException(string message)
        : base(message)
        {
        }
        
        public ReligionsException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}