using AWA.Util.Entity;
using AWA.Util.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AWA.Util.Helpers
{
   public static class HttpHelper
    {
        /// <summary>
        /// 验证远程服务器证书方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //if (sslPolicyErrors == SslPolicyErrors.None)
            return true;

            //验证服务器证书颁发机构及证书名称
            //if ("CN=Meda.cc".Equals(certificate.Issuer) && "CN=Meda.cc".Equals(certificate.Subject))
            //{
            //    //验证服务器证书生效日期及过期时间
            //    var now = DateTime.Now;
            //    if (now >= DateTime.Parse(certificate.GetEffectiveDateString()) && now <= DateTime.Parse(certificate.GetExpirationDateString()))
            //    {
            //        //信任服务器证书
            //        return true;
            //    }
            //}

            //// Do not allow this client to communicate with unauthenticated servers. 
            //return false;
        }

        /// <summary>
        /// http or https post request
        /// </summary>
        /// <param name="requestInput"></param>
        /// <param name="remoteCertificateValidationCallback"></param>
        /// <returns></returns>
        public static string RequestPost(HttpRequestArgs requestInput, Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool> remoteCertificateValidationCallback = null)
        {
            var res = string.Empty;
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(requestInput.Url);
            try
            {
                request.Method = "POST";
                request.ContentType = string.IsNullOrEmpty(requestInput.ContentType) ? "application/x-www-form-urlencoded" : requestInput.ContentType; //application/json; encoding=utf-8
                byte[] byte1 = requestInput.GetBodyBytes().Result;
                request.ContentLength = byte1.Length;

                if (requestInput.TimeOut > 0)
                {
                    request.Timeout = requestInput.TimeOut;
                }

                if (!requestInput.Host.IsNullOrWhiteSpace())
                    request.Host = requestInput.Host;

                if (requestInput.Expect100Continue.HasValue)
                    System.Net.ServicePointManager.Expect100Continue = requestInput.Expect100Continue.Value;

                if (requestInput.KeepAlive.HasValue)
                    request.KeepAlive = requestInput.KeepAlive.Value;

                request.ProtocolVersion = requestInput.HttpVer;

                if (!requestInput.UserAgent.IsNullOrWhiteSpace())
                    request.UserAgent = requestInput.UserAgent;

                if (requestInput.DefaultConnectionLimit.HasValue)
                    ServicePointManager.DefaultConnectionLimit = requestInput.DefaultConnectionLimit.Value;

                if (requestInput.DnsRefreshTimeout.HasValue)
                    ServicePointManager.DnsRefreshTimeout = requestInput.DnsRefreshTimeout.Value;

                foreach (var header in requestInput.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }


                //验证服务器证书回调自动验证  
                ServicePointManager.ServerCertificateValidationCallback = ((remoteCertificateValidationCallback == null) ? new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult) : new System.Net.Security.RemoteCertificateValidationCallback(remoteCertificateValidationCallback));

                using (Stream newStream = request.GetRequestStream())
                {
                    // Send the data.
                    newStream.Write(byte1, 0, byte1.Length);    //写入参数
                }

                using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    Encoding coding = string.IsNullOrEmpty(requestInput.CharSet) ? System.Text.Encoding.UTF8 : System.Text.Encoding.GetEncoding(requestInput.CharSet);
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), coding))
                    {
                        var responseMessage = reader.ReadToEnd();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            res = responseMessage;
                        }

                        response.Close();
                    }
                }


            }
            catch (Exception ex)
            {
                res = string.Format("ERROR:{0}", ex.Message);
            }
            finally
            {

                request.Abort();
                request = null;
            }

            return res;
        }


        /// <summary>
        /// http or https post request
        /// </summary>
        /// <param name="requestInput"></param>
        /// <param name="remoteCertificateValidationCallback"></param>
        /// <returns></returns>
        public static async Task<string> RequestPostAsync(HttpRequestArgs requestInput, Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool> remoteCertificateValidationCallback = null)
        {
            var res = string.Empty;
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(requestInput.Url);
            try
            {
                request.Method = "POST";
                request.ContentType = string.IsNullOrEmpty(requestInput.ContentType) ? "application/x-www-form-urlencoded" : requestInput.ContentType; //application/json; encoding=utf-8
                byte[] byte1 = requestInput.GetBodyBytes().Result;
                request.ContentLength = byte1.Length;

                if (requestInput.TimeOut > 0)
                {
                    request.Timeout = requestInput.TimeOut;
                }

                if (!requestInput.Host.IsNullOrWhiteSpace())
                    request.Host = requestInput.Host;

                if (requestInput.Expect100Continue.HasValue)
                    System.Net.ServicePointManager.Expect100Continue = requestInput.Expect100Continue.Value;

                if (requestInput.KeepAlive.HasValue)
                    request.KeepAlive = requestInput.KeepAlive.Value;

                request.ProtocolVersion = requestInput.HttpVer;

                if (!requestInput.UserAgent.IsNullOrWhiteSpace())
                    request.UserAgent = requestInput.UserAgent;

                if (requestInput.DefaultConnectionLimit.HasValue)
                    ServicePointManager.DefaultConnectionLimit = requestInput.DefaultConnectionLimit.Value;

                if (requestInput.DnsRefreshTimeout.HasValue)
                    ServicePointManager.DnsRefreshTimeout = requestInput.DnsRefreshTimeout.Value;

                foreach (var header in requestInput.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }


                //验证服务器证书回调自动验证  
                ServicePointManager.ServerCertificateValidationCallback = ((remoteCertificateValidationCallback == null) ? new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult) : new System.Net.Security.RemoteCertificateValidationCallback(remoteCertificateValidationCallback));

                using (Stream newStream = await request.GetRequestStreamAsync())
                {
                    // Send the data.
                    newStream.Write(byte1, 0, byte1.Length);    //写入参数
                }

                using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)(await request.GetResponseAsync()))
                {
                    Encoding coding = string.IsNullOrEmpty(requestInput.CharSet) ? System.Text.Encoding.UTF8 : System.Text.Encoding.GetEncoding(requestInput.CharSet);
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), coding))
                    {
                        var responseMessage = reader.ReadToEnd();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            res = responseMessage;
                        }

                        response.Close();
                    }
                }


            }
            catch (Exception ex)
            {
                res = string.Format("ERROR:{0}", ex.Message);
            }
            finally
            {

                request.Abort();
                request = null;
            }

            return res;
        }


        /// <summary>
        /// http or https get request
        /// </summary>
        /// <param name="requestInput"></param>
        /// <param name="remoteCertificateValidationCallback"></param>
        /// <returns></returns>
        public static string RequestGet(HttpRequestArgs requestInput, Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool> remoteCertificateValidationCallback = null)
        {
            var res = string.Empty;
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(requestInput.Url);

                if (requestInput.TimeOut > 0)
                {
                    request.Timeout = requestInput.TimeOut;
                }

                foreach (var header in requestInput.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                //验证服务器证书回调自动验证  
                ServicePointManager.ServerCertificateValidationCallback = ((remoteCertificateValidationCallback == null) ? new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult) : new System.Net.Security.RemoteCertificateValidationCallback(remoteCertificateValidationCallback));
                using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    Encoding coding = string.IsNullOrEmpty(requestInput.CharSet) ? System.Text.Encoding.UTF8 : System.Text.Encoding.GetEncoding(requestInput.CharSet);
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), coding))
                    {
                        var responseMessage = reader.ReadToEnd();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            res = responseMessage;
                        }
                    }
                }

                request = null;
            }
            catch (Exception ex)
            {
                res = string.Format("ERROR:{0}", ex.Message);
            }
            return res;
        }


        /// <summary>
        /// http or https get request
        /// </summary>
        /// <param name="requestInput"></param>
        /// <param name="remoteCertificateValidationCallback"></param>
        /// <returns></returns>
        public static async Task<string> RequestGetAsync(HttpRequestArgs requestInput, Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool> remoteCertificateValidationCallback = null)
        {
            var res = string.Empty;
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(requestInput.Url);

                if (requestInput.TimeOut > 0)
                {
                    request.Timeout = requestInput.TimeOut;
                }

                foreach (var header in requestInput.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                //验证服务器证书回调自动验证  
                ServicePointManager.ServerCertificateValidationCallback = ((remoteCertificateValidationCallback == null) ? new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult) : new System.Net.Security.RemoteCertificateValidationCallback(remoteCertificateValidationCallback));
                using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)(await request.GetResponseAsync()))
                {
                    Encoding coding = string.IsNullOrEmpty(requestInput.CharSet) ? System.Text.Encoding.UTF8 : System.Text.Encoding.GetEncoding(requestInput.CharSet);
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), coding))
                    {
                        var responseMessage = reader.ReadToEnd();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            res = responseMessage;
                        }
                    }
                }

                request = null;
            }
            catch (Exception ex)
            {
                res = string.Format("ERROR:{0}", ex.Message);
            }
            return res;
        }

    }
}
