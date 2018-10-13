namespace TWBuildingAssistant.Model.Climate
{
    using System;
    
    public class ClimateException : Exception
    {
        public ClimateException()
        : base("Failure concerning climates.")
        {
        }
        
        public ClimateException(string message)
        : base(message)
        {
        }
        
        public ClimateException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}