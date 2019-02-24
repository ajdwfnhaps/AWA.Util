using System;
using System.Collections.Generic;
using AWA.Util.WebBase.Extensions;
using Microsoft.AspNetCore.Http;

namespace AWA.Util.WebBase
{
    /// <summary>
    /// web容器共享数据
    /// 只针对单次页面请求有效
    /// </summary>
    public class WebDataContainer
    {
        private static HttpContext current
        {
            get
            {
                return HttpContextExtensions.Current;
            }
        }
        /// <summary>
        /// 清除Items数据
        /// </summary>
        /// <param name="key"></param>
        public static void Clear(string key)
        {
            current.Items[key] = null;
            current.Items[key + "_Loaded"] = false;
        }

        /// <summary>
        /// 批量清除Items数据
        /// </summary>
        public static void ClearAll()
        {
            List<string> list = new List<string>();
            foreach (object obj2 in current.Items.Keys)
            {
                list.Add(obj2.ToString());
            }
            foreach (string str in list)
            {
                current.Items[str] = null;
                current.Items[str + "_Loaded"] = false;
            }
        }

        /// <summary>
        /// 强制设置Items数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, object value)
        {
            current.Items[key] = value;
            current.Items[key + "_Loaded"] = false;
        }

        /// <summary>
        /// 同一个Http请求里，如果键值未被加载过，才调用委托方法设置并获得Items数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="createFn"></param>
        /// <returns></returns>
        public static T TryGet<T>(string key, Func<T> createFn = null)
        {
            bool flag = false;
            if (current.Items[key + "_Loaded"] != null)
            {
                flag = (bool)current.Items[key + "_Loaded"];
            }
            bool flag2 = false;

            flag2 = current.Items.ContainsKey(key);

            if ((!flag && !flag2) && (createFn != null))
            {
                current.Items[key + "_Loaded"] = true;
                current.Items[key] = createFn();
            }
            return (T)current.Items[key];
        }
    }
}
