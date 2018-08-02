namespace TWBuildingAssistant.Model.Resources
{
    using System;

    /// <summary>
    /// The exception that is thrown when an error regarding resources occures.
    /// </summary>
    public class ResourcesException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesException"/> class.
        /// </summary>
        public ResourcesException() 
        : base("Failure concerning resources.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        public ResourcesException(string message) 
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        /// <param name="innerException">
        /// The inner exception that is the cause of this exception.
        /// </param>
        public ResourcesException(string message, Exception innerException) 
        : base(message, innerException)
        {
        }
    }
}