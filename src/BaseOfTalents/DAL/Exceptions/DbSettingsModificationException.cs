using System;

namespace DAL.Exceptions
{
    [Serializable]
    public class DbSettingsModificationException : Exception
    {
        public DbSettingsModificationException() : base("Context settings storage can't be modified any more") { }
        public DbSettingsModificationException(string message, Exception inner) : base(message, inner) { }
        protected DbSettingsModificationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
