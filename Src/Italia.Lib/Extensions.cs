using Dzaba.Utils;
using System;

namespace Italia.Lib
{
    public static class Extensions
    {
        public static Uri GetHostWithScheme(this Uri url)
        {
            Require.NotNull(url, nameof(url));

            var temp = $"{url.Scheme}://{url.Host}";
            return new Uri(temp);
        }
    }
}
