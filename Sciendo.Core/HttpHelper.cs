using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Sciendo.Core
{
    public static class HttpHelper
    {
        public static string Get(string url)
        {
            string response = string.Empty;
            WebRequest webRequest = WebRequest.Create(url);
            try
            {
                using (StreamReader streamObj = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {

                    while (!streamObj.EndOfStream)
                    {
                        response = string.Format("{0}{1}", response, streamObj.ReadLine());
                    }
                }

            }
            catch (WebException wex)
            {
                //log it here
            }
            return response;
        }
    }
}
