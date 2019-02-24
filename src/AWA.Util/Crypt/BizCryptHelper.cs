using AWA.Util.Crypt;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWA.Util.Crypt
{
    /// <summary>
    /// 生产、财务、机器管理系统数据加密存储、解密读取帮助类
    /// </summary>
    public class BizCryptHelper
    {
        /// <summary>
        /// 8位字符的密钥字符串
        /// </summary>
        private const string KEY = "AIMY_KEY";
        /// <summary>
        /// 8位字符的初始化向量字符串
        /// </summary>
        private const string IV = "AIMY__IV";

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="data">加密数据</param>
        /// <returns></returns>
        public static string DESEncrypt(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return string.Empty;
            return CryptHelper.DESEncrypt(data, KEY, IV);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <returns></returns>
        public static string DESDecrypt(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return string.Empty;
            return CryptHelper.DESDecrypt(data, KEY, IV);
        }
    }
}
