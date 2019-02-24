using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AWA.Util.Entity
{
    public class HttpRequestArgs
    {
        public HttpRequestArgs()
        {
            this.Headers = new Dictionary<string, string>();
        }

        ///// <summary>
        ///// 标题
        ///// </summary>
        //public string Title { get; set; }
        /// <summary>
        /// 请求地址资源
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求报文
        /// </summary>
        public virtual HttpContent RequestBody { get; set; }
        /// <summary>
        /// 字符集
        /// </summary>
        public string CharSet { get; set; }

        ///// <summary>
        ///// 来源
        ///// </summary>
        //public string Referer { get; set; }
        /// <summary>
        /// Content-type HTTP 标头的值
        /// 默认值为 application/x-www-form-urlencoded
        /// </summary>
        public string ContentType { get; set; }
        ///// <summary>
        ///// 是否返回Json格式数据
        ///// </summary>
        //public bool IsResponseJson { get; set; }

        /// <summary>
        /// 请求超时时间（毫秒为单位）
        /// </summary>
        public int TimeOut { get; set; }


        /// <summary>
        /// KeepAlive
        /// </summary>
        public bool? KeepAlive { get; set; }

        /// <summary>
        /// HttpVersion
        /// </summary>
        public Version HttpVer { get; set; } = System.Net.HttpVersion.Version11;

        /// <summary>
        /// UserAgent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Expect100Continue
        /// </summary>
        public bool? Expect100Continue { get; set; }

        /// <summary>
        /// DefaultConnectionLimit
        /// </summary>
        public int? DefaultConnectionLimit { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// DnsRefreshTimeout
        /// </summary>
        public int? DnsRefreshTimeout { get; set; }


        /// <summary>
        /// Headers
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <returns></returns>
        public virtual async Task<byte[]> GetBodyBytes()
        {
            return await RequestBody.ReadAsByteArrayAsync();
        }
    }
    /// <summary>
    /// HttpRequestInput
    /// </summary>
    public class HttpRequestInput: HttpRequestArgs
    {
        /// <summary>
        /// 请求报文
        /// </summary>
        public new string RequestBody { get; set; }

        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <returns></returns>
        public override async Task<byte[]> GetBodyBytes()
        {
            byte[] byte1 = Encoding.UTF8.GetBytes(RequestBody);
            return byte1;
        }
    }
}
