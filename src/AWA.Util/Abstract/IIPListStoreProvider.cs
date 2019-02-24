using AWA.Util.Enums;
using System.Collections.Generic;

namespace AWA.Util.Abstract
{
    /// <summary>
    /// IP名单存储提供者
    /// </summary>
    public interface IIPListStoreProvider
    {
        /// <summary>
        /// IP白名单
        /// </summary>
        IList<string> WhiteList { get; }

        /// <summary>
        /// IP黑名单
        /// </summary>
        IList<string> BlackList { get; }

        /// <summary>
        /// 添加IP名单
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="listType">名单类型</param>
        /// <returns></returns>
        bool Add(string ip, IPListType listType = IPListType.Black);
    }
}
