namespace WebUI.Services.Auth
{
    /// <summary>
    /// The provider for new tokens
    /// </summary>
    public static class TokenProvider
    {
        private const int randomStringLength = 30;
        /// <summary>
        /// The function that creates a random uniq token
        /// </summary>
        /// <returns>Token string</returns>
        /// <remarks>
        /// TODO: Consider making this function async if it is needed
        /// </remarks>
        public static string CreateToken()
        {
            string token = Keywielder.Keywielder
                .New()
                .AddRandomString(randomStringLength)
                .AddGUIDString()
                .BuildKey();

            return token;
        }

        //public static Tuple<string, string> ParseToken(string token)
        //{
        //    throw new NotImplementedException();
        //}
    }
}