namespace TWBuildingAssistant.Model.Effects
{
    using System;
    
    public class EffectsException : Exception
    {
        public EffectsException()
        : base("Failure concerning effects.")
        {
        }
        
        public EffectsException(string message)
        : base(message)
        {
        }
        
        public EffectsException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}