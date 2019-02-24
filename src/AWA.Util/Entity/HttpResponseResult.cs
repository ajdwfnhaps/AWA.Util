using AWA.Util.Common.Entity;
using System;
using System.Net;

namespace AWA.Util.Entity
{
    /// <summary>
    /// http 响应结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpResponseResult<T> : Result<T> // ReturnResult
        where T : class
    {
        /// <summary>
        /// 获取响应的状态
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        ///// <summary>
        ///// 返回泛型对象结果
        ///// </summary>
        //public T GenericsResult { get; set; }

        ///// <summary>
        ///// 返回字符串结果
        ///// </summary>
        //public string StringResult { get; set; }

        /// <summary>
        /// 返回泛型结果
        /// </summary>
        //public T Result { get; set; }
    }
}
