using AWA.Util.Auth;
using AWA.Util.Crypt;
using AWA.Util.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AWA.Util.Entity
{
    /// <summary>
    /// 数字签名模型
    /// </summary>
    public class SignatureEntity
    {
        /// <summary>
        /// 外部应用AppKey （需向官方申请获得）
        /// </summary>
        [Required(ErrorMessage = "AppKey不能为空！")]
        public string AppKey { get; set; }

        /// <summary>
        /// 时间截
        /// <example>e.g.：1449121712</example>
        /// </summary>
        [Required(ErrorMessage = "时间截不能为空！")]
        public string Timestamp { get; set; }

        /// <summary>
        /// 随机串
        /// <example>e.g.：jaf6eOJFl32ko</example>
        /// </summary>
        [Required(ErrorMessage = "随机串不能为空！")]
        public string Nonce { get; set; }

        /// <summary>
        /// 签名串
        /// e.g.：应用密钥、时间截、随机串、用户帐号、密码的值组成数组，然后数组ASCII码排序，MD5 32位加密得到
        /// </summary>
        [Required(ErrorMessage = "签名不能为空！")]
        public virtual string Sign { get; set; }

        /// <summary>
        /// 计算签名串
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        public void ComputeSign(string appKey,string appSecret)
        {
            AppKey = appKey;
            Timestamp= DateTime.Now.ToTimeStamp().ToString();
            Nonce = 16.RandString();
            var modelType = this.GetType();
            var otherDatas = new List<string>();
            var properties = modelType.GetProperties();
            foreach (var item in properties)
            {
                var propertiesName = item.Name.ToLower();
                switch (propertiesName)
                {
                    case "appkey":
                    case "sign":
                        break;
                    default:
                        var value = item.GetValue(this);
                        string valueString = null;
                        if (value != null)
                        {
                            valueString = value.ToString();
                            if (!string.IsNullOrEmpty(valueString))
                            {
                                otherDatas.Add(value.ToString());
                            }
                        }
                        break;
                }
            }
            var sign = new ApiKeyAuthBase() { Key = AppKey, Secret = appSecret, EncryptDelegate = CryptHelper.MD5Encrypt }.SignData(otherDatas.ToArray());
            Sign = sign;
        }
    }
}