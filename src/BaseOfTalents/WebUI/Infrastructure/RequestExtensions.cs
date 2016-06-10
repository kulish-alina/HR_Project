using System;
using System.Linq;
using System.Net.Http;

namespace BaseOfTalents.WebUI.Infrastructure
{
    public static class RequestExtensions
    {
        public static int GetPageSize(
            this HttpRequestMessage requestMessage,
            int defaultSize = 5)
        {
            return GetIntFromQueryString(requestMessage, "pageSize", defaultSize);
        }

        public static int GetPageIndex(
            this HttpRequestMessage requestMessage,
            int defaultIndex = 0)
        {
            return GetIntFromQueryString(requestMessage, "pageIndex", defaultIndex);
        }

        public static int GetIntFromQueryString(
            this HttpRequestMessage requestMessage,
            string key,
            int defaultValue)
        {
            var pair = requestMessage
                .GetQueryNameValuePairs()
                .FirstOrDefault(p => p.Key.Equals(key,
                    StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrWhiteSpace(pair.Value))
            {
                int value;
                if (int.TryParse(pair.Value, out value))
                {
                    return value;
                }
            }

            return defaultValue;
        }
    }
}