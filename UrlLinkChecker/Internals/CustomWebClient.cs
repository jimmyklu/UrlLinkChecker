namespace UrlLinkChecker.Internals
{
    using System;
    using System.Net;

    internal class CustomWebClient : WebClient
    {
        private static readonly int TimeoutSeconds;

        private const int SecondsMultiplier = 1000;

        private static readonly int MaxRedirectCount = int.Parse(RscLiterals.WebRequest_MaxRedirectCount);
        private static readonly int RedirectResponseCodeMin = int.Parse(RscLiterals.WebRequest_HeaderRedirectResponseCodeMin);
        private static readonly int RedirectResponseCodeMax = int.Parse(RscLiterals.WebRequest_HeaderRedirectResponseCodeMax);

        
        static CustomWebClient()
        {
            int defaultSeconds = int.Parse(RscLiterals.WebRequest_TimeoutSecondsDefault);

            if (Properties.Settings.Default.WebClientTimeout > 0)
            {
                defaultSeconds = Properties.Settings.Default.WebClientTimeout;
            }
            TimeoutSeconds = defaultSeconds;
        }

        internal bool HeadOnly { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest req = base.GetWebRequest(address);
            if (HeadOnly && req.Method == RscLiterals.WebRequest_HeaderComparedValue)
            {
                req.Method = RscLiterals.WebRequest_HeaderUpdatedValue;
            }
            req.Timeout = TimeoutSeconds * SecondsMultiplier;
            return req;
        }


        internal string GetUrl(string url, out int followCount, int redirectCount = 0)
        {
            followCount = redirectCount;

            WebRequest req = base.GetWebRequest(new Uri(url));
            req.Timeout = TimeoutSeconds * SecondsMultiplier;
            HttpWebResponse response = null;

            try
            {
                int nextCount = redirectCount + 1;

                response = (HttpWebResponse)req.GetResponse();

                int respCode = (int)response.StatusCode;

                if (nextCount < MaxRedirectCount && respCode >= RedirectResponseCodeMin && respCode < RedirectResponseCodeMax)
                {
                    var nextUrl = response.ResponseUri.ToString();
                    return GetUrl(nextUrl, out followCount, nextCount);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                req.Method = RscLiterals.WebRequest_HeaderUpdatedValue;
                return base.DownloadString(url);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }
    }
}
