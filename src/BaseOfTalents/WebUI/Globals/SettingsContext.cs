namespace WebUI.Globals
{
    [System.Serializable]
    public class SettingsModificationException : System.Exception
    {
        public SettingsModificationException() : base("Not allowed modification of application settings context") { }
        public SettingsModificationException(string message) : base(message) { }
        public SettingsModificationException(string message, System.Exception inner) : base(message, inner) { }
        protected SettingsModificationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public sealed class SettingsContext
    {
        private static volatile SettingsContext instance;
        private static object syncRoot = new object();

        private string _hostUrl;
        private string _issuerUrl;
        private int _port;

        private string _email;
        private string _password;

        private string _secret;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsContext"/> class.
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="remoteUrl"></param>
        /// <param name="port"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="secret"></param>
        private SettingsContext(string hostUrl, string remoteUrl, int port, string email, string password,
            string secret)
        {
            _hostUrl = hostUrl;
            _issuerUrl = remoteUrl;
            _port = port;
            _secret = secret;

            _email = email;
            _password = password;
        }

        /// <summary>
        /// Creates an instance of <see cref="SettingsContext"/> singleton.
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="remoteUrl"></param>
        /// <param name="port"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="secret"></param>
        public static void SetInstance(string hostUrl, string remoteUrl, int port, string email, string password,
            string secret)
        {
            if (instance != null)
            {
                throw new SettingsModificationException();
            }

            instance = new SettingsContext(hostUrl, remoteUrl, port, email, password, secret);
        }

        /// <summary>
        /// The only instance of <see cref="SettingsContext"/> class.
        /// </summary>
        public static SettingsContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Url of where to host the server
        /// </summary>
        public string HostUrl
        {
            get
            {
                return _hostUrl;
            }
        }

        /// <summary>
        /// Url of server from side of client (where the issuer is located)
        /// </summary>
        public string IssuerUrl
        {
            get
            {
                return _issuerUrl;
            }
        }

        /// <summary>
        /// Port on which hosting should start
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }
        }

        /// <summary>
        /// Root where is located content of the website
        /// </summary>
        public string WWWRoot
        {
            get
            {
                return "wwwroot";
            }
        }

        /// <summary>
        /// Folder where will be placed files uploaded by users
        /// </summary>
        public string Uploads
        {
            get
            {
                return $"{WWWRoot}/uploads";
            }
        }

        /// <summary>
        /// Email from which email invitations would be sent
        /// <seealso cref="Mailer.MailAgent.Send(string, string, string)"/>
        /// </summary>
        public string Email
        {
            get
            {
                return _email;
            }
        }

        /// <summary>
        /// Password for the <see cref="Email"/>. Is emtpy if smtp server doesn't require authorization.
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
        }

        /// <summary>
        /// Relative url on where
        /// </summary>
        public string RequestPath
        {
            get
            {
                return "/uploads";
            }
        }

        /// <summary>
        /// Relative url to image for invitational email
        /// </summary>
        public string ImageUrl
        {
            get
            {
                return "/isdmail.png";
            }
        }

        /// <summary>
        /// Relative url to a location on which user should be sent with click on the email link
        /// </summary>
        public string OuterUrl
        {
            get
            {
                return "/login";
            }
        }

        public string Secret
        {
            get
            {
                return _secret;
            }
        }
    }
}