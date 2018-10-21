namespace TWBuildingAssistant.Model.Map
{
    using System;
    
    public class MapException : Exception
    {
        public MapException()
        : base("Failure concerning the map.")
        {
        }
        
        public MapException(string message)
        : base(message)
        {
        }
        
        public MapException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}