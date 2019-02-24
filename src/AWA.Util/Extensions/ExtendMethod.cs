#if NETCORE
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
#endif


using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AWA.Util.Extensions
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    public static partial class ExtendMethod
    {

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileExtenName(this string fileName)
        {
            string[] temp = fileName.Split('.');
            if (temp.Length < 2) { return string.Empty; }
            return temp[temp.Length - 1].ToLower();
        }

        /// <summary>
        /// 返回请求url地址格式字符串
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static string ToUrlParamString(this NameValueCollection cols)
        {
            if (cols.Count < 1) { return string.Empty; }

            StringBuilder sb = new StringBuilder();

            foreach (var key in cols.AllKeys)
            {
                sb.AppendFormat("&{0}={1}", key, cols[key]);
            }

            var res = sb.ToString();
            if (!string.IsNullOrWhiteSpace(res)) res = res.Substring(1);

            return res;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }


        /// <summary>
        /// 字节长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ByteLength(this string str)
        {
            throw new NotImplementedException();
            //return System.Text.Encoding.Default.GetByteCount(str);
        }

        /// <summary>
        /// 返回空字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DefaultEmpty(this string key)
        {
            if (string.IsNullOrEmpty(key)) { return string.Empty; }
            return key;
        }

        /// <summary>
        /// GenerateStringID
        /// </summary>
        /// <returns></returns>
        public static string GenerateStringID()
        {

            long i = 1;

            foreach (byte b in Guid.NewGuid().ToByteArray())
            {

                i *= ((int)b + 1);

            }

            return string.Format("{0:x}", i - DateTime.Now.Ticks);

        }

        /// <summary>
        /// 通过GUID生成一个唯一19位长的序列
        /// </summary>
        /// <returns></returns>
        public static long GenerateIntID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();

            return BitConverter.ToInt64(buffer, 0);
        }


        /// <summary>
        /// 获取Enum 描述信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo item = type.GetField(value.ToString(), BindingFlags.Public | BindingFlags.Static);
            if (item == null) return null;
            var attribute = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute != null && !String.IsNullOrEmpty(attribute.Description)) return attribute.Description;
            return string.Empty;
        }


        /// <summary>
        /// 获取Flags Enum 描述信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ICollection<string> GetFlagsDescription(this Enum value)
        {
            Type type = value.GetType();
            var arr = value.ToString()
                .Replace(" ", string.Empty)
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var res = new List<string>();

            foreach (var str in arr)
            {
                FieldInfo item = type.GetField(str, BindingFlags.Public | BindingFlags.Static);
                if (item == null) continue;
                var attribute = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null && !String.IsNullOrEmpty(attribute.Description))
                    res.Add(attribute.Description);
            }
            return res;
        }


        /// <summary>
        /// string to enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string key)
            where T : struct
        {
            if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), key);
            }
            return default(T);
        }

        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandString(this int length)
        {
            string str = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";//75－13个字符 减 ~!@#$%^&*()_+
            Random r = new Random(GetRandomSeed());
            string result = string.Empty;

            //生成一个8位长的随机字符，具体长度可以自己更改
            for (int i = 0; i < length; i++)
            {
                int m = r.Next(0, 62);//这里下界是0，随机数可以取到，上界应该是75，因为随机数取不到上界，也就是最大74，符合我们的题意
                string s = str.Substring(m, 1);
                result += s;
            }

            return result;
        }

        /// <summary>
        /// 随机整型字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandIntString(this int length)
        {
            Random rd = new Random(GetRandomSeed());
            string str = string.Empty;
            while (str.Length < length)
            {
                int temp = rd.Next(0, 10);
                //if (!str.Contains(temp + ""))
                {
                    str += temp;
                }
            }
            return str;
        }

        private static int GetRandomSeed()
        {
            long tick = DateTime.Now.Ticks;
            return (int)(tick & 0xffffffffL) | (int)(tick >> 32);

            //byte[] bytes = new byte[4];
            //System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            //rng.GetBytes(bytes);
            //return BitConverter.ToInt32(bytes, 0);
        }

#if NETCORE
        /// <summary>
        /// 获取配置信息值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ValueOfConfig(this string key)
        {
            return key.SectionOfConfig().Value;
        }

        public static IConfigurationSection SectionOfConfig(this string key)
        {
            return GlobalVariable.Configuration.GetSection(key);
        }

        

        
#endif
        
        /// <summary>
        /// TryToInt
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int TryToInt(this string key)
        {
            int res = 0;
            int.TryParse(key, out res);
            return res;
        }

        /// <summary>
        /// TryToDouble
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static double TryToDouble(this string key)
        {
            double res = 0;
            double.TryParse(key, out res);
            return res;
        }

        /// <summary>
        /// TryToDecimal
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal TryToDecimal(this string key)
        {
            decimal res = 0m;
            decimal.TryParse(key, out res);
            return res;
        }

        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampToDateTime(this long timeStamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddSeconds(timeStamp).AddHours(8);
        }

        /// <summary>
        /// Unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        public static long UnixTicks(this DateTime dt)
        {
            var unixTimestampOrigin = new DateTime(1970, 1, 1);
            return (long)new TimeSpan(dt.Ticks - unixTimestampOrigin.Ticks).TotalMilliseconds;
        }

        public static DateTime? ToDateTime(this DateTimeOffset? offset)
        {
            if (offset.HasValue)
            {
                return offset.Value.DateTime;
            }

            return null;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampToDateTime(this string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);

            return dateTimeStart.Add(toNow);
        }


        /// <summary>
        /// 显示数组缩略信息
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string BriefFromArr(this string[] arr, int count)
        {
            var moreStr = "";
            if (arr.Length > count)
            {
                arr = arr.Take(3).ToArray();
                moreStr = "...";
            }
            return string.Join(",", arr) + moreStr;
        }

        /// <summary>
        /// 转换为md5 32位密文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5(this string input)
        {
            //string cl = input;
            //string pwd = "";
            //MD5 md5 = MD5.Create();//实例化一个md5对像
            //// 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            //byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            //// 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            //for (int i = 0; i < s.Length; i++)
            //{
            //    // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
            //    pwd = pwd + s[i].ToString("x");

            //}
            //return pwd;

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(input)));
            t2 = t2.Replace("-", "");
            return t2;
        }
    }
}
