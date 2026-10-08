using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace HxSDK.TicketCode.Helpers
{
    internal class HttpHelper
    {
        public static HttpStatusCode Post(string uri, string param, WebHeaderCollection headers, out string result, out string error, string encoding = "utf-8")
        {
            HttpStatusCode status;
            result = "";
            error = "";
            HttpWebRequest length = WebRequest.Create(uri) as HttpWebRequest;
            length.Method = "post";
            Encoding encoding1 = Encoding.GetEncoding(encoding);
            try
            {
                length.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                length.KeepAlive = true;
                if (headers != null)
                {
                    if (!string.IsNullOrWhiteSpace(headers.Get("Host")))
                    {
                        length.Host = headers.Get("Host");
                    }
                    if (!string.IsNullOrWhiteSpace(headers.Get("Content-Type")))
                    {
                        length.ContentType = headers.Get("Content-Type");
                    }
                    if (!string.IsNullOrWhiteSpace(headers.Get("Referer")))
                    {
                        length.Referer = headers.Get("Referer");
                    }
                    if (!string.IsNullOrWhiteSpace(headers.Get("Accept")))
                    {
                        length.Accept = headers.Get("Accept");
                    }
                    if (!string.IsNullOrWhiteSpace(headers.Get("UserAgent")))
                    {
                        length.UserAgent = headers.Get("UserAgent");
                    }
                    headers.Remove("Host");
                    headers.Remove("Content-Type");
                    headers.Remove("Referer");
                    headers.Remove("Accept");
                    headers.Remove("UserAgent");
                    foreach (string header in headers)
                    {
                        length.Headers.Add(header, headers[header]);
                    }
                }
                length.ContentLength = 0;
                if (!string.IsNullOrWhiteSpace(param))
                {
                    byte[] bytes = encoding1.GetBytes(param);
                    length.ContentLength = (long)((int)bytes.Length);
                    length.GetRequestStream().Write(bytes, 0, (int)bytes.Length);
                }
                using (WebResponse response = length.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        result = (new StreamReader(responseStream, encoding1)).ReadToEnd();
                        status = HttpStatusCode.OK;
                    }
                }
            }
            catch (WebException webException)
            {
                error = string.Concat(webException.Message, Environment.NewLine, webException.StackTrace);
                status = (HttpStatusCode)webException.Status;
                if (webException.Response != null)
                {
                    using (var response = webException.Response as HttpWebResponse)
                    {
                        if (response != null)
                        {
                            status = response.StatusCode;
                            using (var stream = webException.Response.GetResponseStream())
                            {
                                error = new StreamReader(stream).ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                error = string.Concat(exception.Message, Environment.NewLine, exception.StackTrace);
                status = HttpStatusCode.InternalServerError;
            }
            return status;
        }
    }
}
