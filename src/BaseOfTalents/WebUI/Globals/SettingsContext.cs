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

        private string _url;
        private int _port;
        private string _wwwroot;
        private string _uploads;

        private string _email;
        private string _password;

        private SettingsContext(string url, int port, string wwwroot,
            string uploads, string email, string password)
        {
            _url = url;
            _port = port;

            _wwwroot = wwwroot;
            _uploads = uploads;
            _email = email;
            _password = password;
        }

        public static void SetInstance(string url, int port, string wwwroot,
            string uploads, string email, string password)
        {
            if (instance != null)
            {
                throw new SettingsModificationException();
            }

            instance = new SettingsContext(url, port, wwwroot, uploads, email, password);
        }

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

        public string Url
        {
            get
            {
                return _url;
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }
        }

        public string WWWRoot
        {
            get
            {
                return _wwwroot;
            }
        }

        public string Uploads
        {
            get
            {
                return _uploads;
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
        }
    }
}