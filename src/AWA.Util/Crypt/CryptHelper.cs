using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AWA.Util.Crypt
{
    /// <summary>
    /// 加密解密
    /// </summary>
    public class CryptHelper
    {
        #region Base64加密解密
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input)
        {
            return Base64Encrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <param name="encode">字符编码</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input, Encoding encode)
        {
            return Convert.ToBase64String(encode.GetBytes(input));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input)
        {
            return Base64Decrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input, Encoding encode)
        {
            return encode.GetString(Convert.FromBase64String(input));
        }
        #endregion

        #region DES加密解密
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="data">加密数据</param>
        /// <param name="key">8位字符的密钥字符串</param>
        /// <param name="iv">8位字符的初始化向量字符串</param>
        /// <returns></returns>
        public static string DESEncrypt(string data, string key, string iv)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(iv);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <param name="key">8位字符的密钥字符串(需要和加密时相同)</param>
        /// <param name="iv">8位字符的初始化向量字符串(需要和加密时相同)</param>
        /// <returns></returns>
        public static string DESDecrypt(string data, string key, string iv)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(iv);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns></returns>
        public static string MD5Encrypt(string input)
        {
            return MD5Encrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string MD5Encrypt(string input, Encoding encode)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(encode.GetBytes(input));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            return sb.ToString();
        }

        /// <summary>
        /// MD5对文件流加密
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static string MD5Encrypt(Stream stream)
        {
            MD5 md5serv = MD5CryptoServiceProvider.Create();
            byte[] buffer = md5serv.ComputeHash(stream);
            StringBuilder sb = new StringBuilder();
            foreach (byte var in buffer)
                sb.Append(var.ToString("x2"));
            return sb.ToString();
        }

        /// <summary>
        /// MD5加密(返回16位加密串)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string input, Encoding encode)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string result = BitConverter.ToString(md5.ComputeHash(encode.GetBytes(input)), 4, 8);
            result = result.Replace("-", "");
            return result;
        }
        #endregion

        #region 3DES 加密解密

        public static string DES3Encrypt(string data, string key)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

            DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
            DES.Mode = CipherMode.CBC;
            DES.Padding = PaddingMode.PKCS7;

            ICryptoTransform DESEncrypt = DES.CreateEncryptor();

            byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(data);
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        public static string DES3Decrypt(string data, string key)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

            DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
            DES.Mode = CipherMode.CBC;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            ICryptoTransform DESDecrypt = DES.CreateDecryptor();

            string result = "";
            try
            {
                byte[] Buffer = Convert.FromBase64String(data);
                result = ASCIIEncoding.ASCII.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {

            }
            return result;
        }

        #endregion

        #region RSA加密算法

        #region 对称
        /// <summary>
        /// 对称加密
        /// </summary>
        /// <param name="data">加密数据</param>
        /// <param name="key">对称密匙</param>
        /// <param name="encode">字符编码</param>
        /// <returns></returns>
        public static string RSAEncryptSym(string data, string key, Encoding encode = null)
        {
            var param = new CspParameters();
            param.KeyContainerName = key;//密匙容器的名称，保持加密解密一致才能解密成功
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                encode = encode ?? Encoding.Default;
                byte[] plaindata = encode.GetBytes(data);//将要加密的字符串转换为字节数组
                byte[] encryptdata = rsa.Encrypt(plaindata, false);//将加密后的字节数据转换为新的加密字节数组
                return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为字符串
            }
        }

        /// <summary>
        /// 对称解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <param name="key">对称密匙</param>
        /// <param name="encode">字符编码</param>
        /// <returns></returns>
        public static string RSADecryptSym(string data, string key, Encoding encode = null)
        {
            var param = new CspParameters();
            param.KeyContainerName = key;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] encryptdata = Convert.FromBase64String(data);
                byte[] decryptdata = rsa.Decrypt(encryptdata, false);
                encode = encode ?? Encoding.Default;
                return encode.GetString(decryptdata);
            }
        }
        #endregion

        #region 非对称

        /// <summary>
        /// 生成密匙文件对
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encode">字符编码</param>
        public static void GenerateKeyFile(string path, Encoding encode = null)
        {
            encode = encode ?? Encoding.Default;
            //公钥在a.txt文件中,私钥在b.txt文件中.制造公钥和私钥的方法如下:
            RSACryptoServiceProvider crypt = new RSACryptoServiceProvider();
            string publickey = crypt.ToXmlString(false);//公钥
            string privatekey = crypt.ToXmlString(true);//私钥
            crypt.Clear();
            //写入文本文件中
            StreamWriter one = new StreamWriter(path + "publickey.xml", true, encode);
            one.Write(publickey);
            StreamWriter two = new StreamWriter(path + "privatekey.xml", true, encode);
            two.Write(privatekey);
            one.Flush();
            two.Flush();
            one.Close();
            two.Close();
        }

        /// <summary>
        /// 非对称加密
        /// </summary>
        /// <param name="data">加密数据</param>
        /// <param name="publicKeyPath">公钥文件路径</param>
        /// <param name="encode">字符编码</param>
        /// <returns></returns>
        public static string RSAEncryptAsy(string data, string publicKeyPath, Encoding encode = null)
        {
            encode = encode ?? Encoding.Default;
            StreamReader sr = new StreamReader(publicKeyPath, encode);
            string readpublickey = sr.ReadToEnd(); //包含 RSA 密钥信息的 XML 字符串。
            sr.Close();

            using (RSACryptoServiceProvider crypt = new RSACryptoServiceProvider())
            {
                byte[] bytes = encode.GetBytes(data);
                crypt.FromXmlString(readpublickey);
                bytes = crypt.Encrypt(bytes, false);
                return Convert.ToBase64String(bytes);
                //string abb = Server.UrlEncode(encryttext);
            }
        }


        /// <summary>
        /// 非对称解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <param name="privateKeyPath">私钥文件路径</param>
        /// <param name="encode">字符编码</param>
        /// <returns></returns>
        public static string RSADecryptAsy(string data, string privateKeyPath, Encoding encode = null)
        {
            encode = encode ?? Encoding.Default;
            StreamReader sr = new StreamReader(privateKeyPath, encode);
            string readprivatekey = sr.ReadToEnd();
            sr.Close();

            using (RSACryptoServiceProvider crypt = new RSACryptoServiceProvider())
            {
                byte[] bytes = Convert.FromBase64String(data);
                crypt.FromXmlString(readprivatekey);
                byte[] decryptbyte = crypt.Decrypt(bytes, false);
                return encode.GetString(decryptbyte);
            }
        }

        #endregion

        #endregion


        #region CreateMachineKey
        /// <summary>
        /// 使用加密服务提供程序实现加密生成随机数
        /// </summary>
        /// <param name="length"></param>
        /// <returns>16进制格式字符串</returns>
        public static string CreateMachineKey(int length)
        {
            // 要返回的字符格式为16进制,byte最大值255
            // 需要2个16进制数保存1个byte,因此除2
            byte[] random = new byte[length / 2];
            // 使用加密服务提供程序 (CSP) 提供的实现来实现加密随机数生成器 (RNG)
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            // 用经过加密的强随机值序列填充字节数组
            rng.GetBytes(random);
            StringBuilder machineKey = new StringBuilder(length);
            for (int i = 0; i < random.Length; i++)
            {
                machineKey.Append(string.Format("{0:X2}", random[i]));
            }
            return machineKey.ToString();
        }
        #endregion
    }
}
