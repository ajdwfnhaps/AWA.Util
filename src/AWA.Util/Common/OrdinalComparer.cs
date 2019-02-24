using System;
using System.Collections.Generic;

namespace AWA.Util.Common
{
    /// <summary>
    /// 字符串字典序排序比较器
    /// </summary>
    public class OrdinalComparer : IComparer<String>
    {
        /// <summary>
        /// 字符串字典序排序比较器
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(String x, String y)
        {
            return string.CompareOrdinal(x, y);
        }
    }
}
