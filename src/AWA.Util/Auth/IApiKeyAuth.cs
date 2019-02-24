using AWA.Util.Common.Entity;
using System.Collections.Generic;

namespace AWA.Util.Auth
{
    /// <summary>
    /// Digital Signature
    /// 数字签名
    /// </summary>
    public interface IApiKeyAuth
    {
        /// <summary>
        /// 应用Key
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 数字签名
        /// </summary>
        /// <param name="srcData">原文</param>
        /// <returns>签名</returns>
        string SignData(string srcData);

        /// <summary>
        /// 数字签名
        /// </summary>
        /// <param name="srcDataArr">数组原文</param>
        /// <returns></returns>
        string SignData(string[] srcDataArr);

        /// <summary>
        /// 验证数字签名
        /// </summary>
        /// <param name="appKey">应用Key</param>
        /// <param name="timeStamp">时间截</param>
        /// <param name="nonce">随机字符串</param>
        /// <param name="signature">签名串</param>
        /// <param name="extendDataArr">扩展签名数据</param>
        /// <returns></returns>
        Result VerifyData(string appKey, string timeStamp, string nonce, string signature, List<string> extendDataArr = null);
    }
}
