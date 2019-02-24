using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace AWA.Util.WebBase.Extensions
{
    public static class HttpExtensions
    {
        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }


        /// <summary>
        /// ValueOfQueryString
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ValueOfQueryString(this string key)
        {
            return HttpContextExtensions.Current.Request.Query[key].ToString();
        }

        /// <summary>
        /// VallueOfQueryStringToInt
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int VallueOfQueryStringToInt(this string key)
        {
            var res = 0;
            int.TryParse(key.ValueOfQueryString(), out res);
            return res;
        }

        /// <summary>
        /// ValueOfForm
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ValueOfForm(this string key)
        {
            return HttpContextExtensions.Current.Request.Form[key].ToString();
        }


    }
}
