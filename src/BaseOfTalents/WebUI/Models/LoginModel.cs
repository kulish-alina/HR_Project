namespace WebUI.Models
{
    /// <summary>
    /// Model for user login credentials
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// The security string of user account
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// User uniq identificator
        /// </summary>
        public string Login { get; set; }
    }
}