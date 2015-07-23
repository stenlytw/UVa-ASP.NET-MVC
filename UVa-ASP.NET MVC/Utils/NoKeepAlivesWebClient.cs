using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace UVa_ASP.NET_MVC.Utils
{
    public class NoKeepAlivesWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                ((HttpWebRequest)request).KeepAlive = false;
            }

            return request;
        }
    }
}