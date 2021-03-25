namespace VirtualSports.Web.Models
{
    /// <summary>
    /// Easy readable error model.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Error code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initializes new instance of the Error class.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public Error(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}