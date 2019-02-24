using AWA.Util.Common;
using AWA.Util.Common.Entity;
using AWA.Util.Entity;
using AWA.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AWA.Util.Auth
{
    /// <summary>
    /// 数字签名基类
    /// </summary>
    public class ApiKeyAuthBase
    {
        /// <summary>
        /// 应用Key
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        public virtual string Secret { get; set; }

        /// <summary>
        /// 签名失效时间（单位：分钟）
        /// 默认10分钟
        /// </summary>
        public int ExpiredMinutes { get; set; }

        /// <summary>
        /// 数字签名基类
        /// </summary>
        public ApiKeyAuthBase()
        {
            this.ExpiredMinutes = 10;
        }

        /// <summary>
        /// 加密方法委托
        /// </summary>
        public virtual Func<string, string> EncryptDelegate { get; set; }

        /// <summary>
        /// 根据App Key获取接入渠道的密钥委托
        /// </summary>
        public virtual Func<string, string> GetSecretDelegate { get; set; }

        /// <summary>
        /// 数字签名－原文
        /// </summary>
        /// <param name="srcData">原文</param>
        /// <returns></returns>
        public virtual string SignData(string srcData)
        {
            if (string.IsNullOrWhiteSpace(srcData)) throw new Exception("加密原文不能为空");

            if (EncryptDelegate == null) throw new Exception("加密方法未实现");

            return EncryptDelegate(srcData);
        }

        /// <summary>
        /// 数字签名－数组原文
        /// </summary>
        /// <param name="srcDataArr">数组原文(不包括接入渠道的密钥)</param>
        /// <returns></returns>
        public virtual string SignData(string[] srcDataArr)
        {
            if (string.IsNullOrWhiteSpace(this.Secret)) throw new Exception("请先设置内部密钥secret");

            var tempList = srcDataArr.ToList();
            tempList.Add(this.Secret);
            OrdinalComparer comp = new OrdinalComparer();
            var tempArr = tempList.ToArray().OrderBy(t => t, comp);
            var srcData = string.Join("", tempArr);
            return this.SignData(srcData);
        }


        /// <summary>
        /// 验证数字签名
        /// </summary>
        /// <param name="appKey">应用Key</param>
        /// <param name="timeStamp">时间截</param>
        /// <param name="nonce">随机字符串</param>
        /// <param name="signature">签名串</param>
        /// <param name="extendDataArr">扩展签名数据</param>
        /// <returns></returns>
        public virtual Result VerifyData(string appKey, string timeStamp, string nonce, string signature, List<string> extendDataArr = null)
        {
            var res = new Result() { Title = "验证数字签名" };

            #region 验证参数完整性
            if (string.IsNullOrWhiteSpace(appKey))
            {
                res.Msg = "AppKey不能为空";
                return res;
            }

            if (string.IsNullOrWhiteSpace(timeStamp))
            {
                res.Msg = "时间截不能为空";
                return res;
            }

            if (string.IsNullOrWhiteSpace(nonce))
            {
                res.Msg = "随机串不能为空";
                return res;
            }

            if (string.IsNullOrWhiteSpace(signature))
            {
                res.Msg = "签名不能为空";
                return res;
            }
            #endregion

            if (GetSecretDelegate == null) throw new Exception("获取内部密钥方法未实现");

            var secret = GetSecretDelegate(appKey);

            if (string.IsNullOrWhiteSpace(secret))
            {
                res.Msg = "不是受权的应用，请勿非法操作！";
                return res;
            }

            var tempList = new List<string> { secret, timeStamp, nonce };
            if (extendDataArr != null && extendDataArr.Count > 0)
            {
                tempList.AddRange(extendDataArr);
            }

            OrdinalComparer comp = new OrdinalComparer();
            var tempArr = tempList.ToArray().OrderBy(t => t, comp);
            string srcDataStr = string.Join("", tempArr);

            if (EncryptDelegate == null) throw new Exception("加密方法未实现");

            var tmpsignature = EncryptDelegate(srcDataStr);

            if (tmpsignature != signature)
            {
                res.Msg = string.Format("无效的数字签名。接收加密签名串：{0}，原签名串：{1}, Api站点加密得到签名串：{2}", signature, srcDataStr, tmpsignature);

                return res;
            }

            if (timeStamp.IsNumeric())
            {
                DateTime dtTime = timeStamp.StampToDateTime();
                double minutes = DateTime.Now.Subtract(dtTime).TotalMinutes;
                if (minutes > ExpiredMinutes)
                {
                    res.Msg = "签名时间戳失效";
                    return res;
                }

                res.Success = true;
                res.Msg = "验证数字签名成功";
            }

            return res;
        }
    }
}
