namespace TWBuildingAssistant.Model.Buildings
{
    using System;

    public class BuildingsException : Exception
    {
        public BuildingsException()
            : base("Failure concerning buildings.")
        {
        }

        public BuildingsException(string message)
            : base(message)
        {
        }

        public BuildingsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}