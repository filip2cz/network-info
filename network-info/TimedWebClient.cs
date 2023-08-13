using System.Net;
using System;

namespace network_info
{
    public class TimedWebClient : WebClient
    {
        public int Timeout { get; set; }

        public TimedWebClient() {
            Timeout = 2000;
        }

        protected override WebRequest GetWebRequest(Uri address) {
            var objWebRequest = base.GetWebRequest(address);
            if (objWebRequest == null) return null;
            objWebRequest.Timeout = Timeout;
            return objWebRequest;
        }
    }
}