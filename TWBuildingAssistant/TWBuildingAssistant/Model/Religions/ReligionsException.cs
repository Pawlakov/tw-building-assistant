
namespace TWBuildingAssistant.Model.Religions
{
    using System;

    /// <summary>
    /// The exception that is thrown when an error regarding religions occures.
    /// </summary>
    public class ReligionsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReligionsException"/> class.
        /// </summary>
        public ReligionsException()
        : base("Failure concerning religions.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReligionsException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        public ReligionsException(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReligionsException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        /// <param name="innerException">
        /// The inner exception that is the cause of this exception.
        /// </param>
        public ReligionsException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}