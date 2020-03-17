using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Common.Extension;

namespace Common
{
    public class HttpHelper
    {
        public string GetHttpContentType(string ext)
        {
            string cType = "image/jpeg";
            switch (ext.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    cType = "image/jpeg";
                    break;
                case ".png":
                    cType = "image/png";
                    break;
                case ".bmp":
                    cType = "image/bmp";
                    break;
                case ".gif":
                    cType = "image/gif";
                    break;
                default:
                    cType = "application/octet-stream";
                    break;
            }
            return cType;
        }

        /// <summary>
        /// 模拟http post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string CreateHttpPostResponse(string url, IDictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = null;
            //如果是发送HTTPS请求   
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";

            request.Headers.Add("X_REG_CODE", "288a633ccc1");
            request.Headers.Add("X_MACHINE_ID", "a306b7c51254cfc5e22c7ac0702cdf87");
            request.Headers.Add("X_REG_SECRET", "de308301cf381bd4a37a184854035475d4c64946");
            request.Headers.Add("X_STORE", "0001");
            request.Headers.Add("X_BAY", "0001-01");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Accept-Language", "zh-CN");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Accept = "*/*";
            request.ContentType = "application/x-www-form-urlencoded";


            request.CookieContainer = new CookieContainer();

            //如果需要POST数据   
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }

                //Encoding encoding = Encoding.GetEncoding("gb2312");
                Encoding encoding = Encoding.GetEncoding("utf-8");
                byte[] data = encoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            Stream s = res.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            //读取服务器端返回的消息 
            string sReturnString = sr.ReadLine();
            return sReturnString;
        }


        public static string CreateHttpGetResponse(string url, IDictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = null;

            StringBuilder buffer = new StringBuilder();
            if (!(parameters == null || parameters.Count == 0))
            {
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("?{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
            }
            //如果是发送HTTPS请求   
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url + buffer.ToString()) as HttpWebRequest;
            }
            request.Method = "GET";

            request.Headers.Add("X_REG_CODE", "288a633ccc1");
            request.Headers.Add("X_MACHINE_ID", "a306b7c51254cfc5e22c7ac0702cdf87");
            request.Headers.Add("X_REG_SECRET", "de308301cf381bd4a37a184854035475d4c64946");
            request.Headers.Add("X_STORE", "0001");
            request.Headers.Add("X_BAY", "0001-01");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Accept-Language", "zh-CN");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Accept = "*/*";


            request.CookieContainer = new CookieContainer();


            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            Stream s = res.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            //读取服务器端返回的消息 
            string sReturnString = sr.ReadLine();
            return sReturnString.Replace("\"", "");
        }


        public static string Post(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

            request.Method = "POST";
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded"; //

            //request.ContentType = "text/xml";
            request.Accept = "";
            Stream outstream = request.GetRequestStream();
            outstream.Close();
            HttpWebResponse hwrp = (HttpWebResponse)request.GetResponse();
            string ContentType = hwrp.Headers.Get("Content-Type");

            StreamReader sr = null;

            sr = new StreamReader(hwrp.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));

            return sr.ReadToEnd();
        }

        public static string Get(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public static string Post2(string postURL)
        {
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postURL);

                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
                request.Headers.Add("Purpose", @"prefetch");
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh,zh-CN;q=0.9,en;q=0.8,zh-TW;q=0.7");
                //request.Headers.Set(HttpRequestHeader.Cookie, @"Hm_lvt_cdce8cda34e84469b1c8015204129522=1569467668,1569547207,1570797708,1570866058");

                response = (HttpWebResponse)request.GetResponse();
                return "1";
                //return response.Headers[HttpRequestHeader.Cookie].ToString();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return "";
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                return "";
            }
            return "";
        }

        /// <summary>
        /// 设置https证书校验机制,默认返回True,验证通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受
        }

        /// <summary>
        /// 模拟http post请求(ContentType为pplication/json)
        /// </summary>
        /// <param name="url">请求的链接</param>
        /// <param name="json">序列化后的对象</param>
        public static string HttpPostJson(string url, string json)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = null;
            //如果是发送HTTPS请求   
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Timeout = 99999999;
            request.KeepAlive = true;
            request.Method = "POST";
            request.Headers.Add("Accept-Language", "zh-CN");
            request.Accept = "*/*";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json;charset=UTF-8";

            request.CookieContainer = new CookieContainer();

            //如果需要POST数据   
            if (!json.IsEmpty())
            {
                StringBuilder buffer = new StringBuilder();
                buffer.Append(json);
                Encoding encoding = Encoding.GetEncoding("utf-8");
                byte[] data = encoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            Stream s = res.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            //读取服务器端返回的消息 
            string sReturnString = sr.ReadLine();
            //if (!string.IsNullOrEmpty(sReturnString))
            //{
            //    sReturnString = sReturnString.Replace("\"", "");
            //}
            return sReturnString;
        }

    }
}
