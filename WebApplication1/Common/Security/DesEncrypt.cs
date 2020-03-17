using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Security
{
    public class DesEncrypt
    {
        private const string DesKey = "En_Taor_Xsxaeas!";

        #region ========加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            return Encrypt(text, DesKey);
        }

        /// <summary> 
        /// 加密数据(可逆) 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string text, string sKey)
        {
            if (string.IsNullOrWhiteSpace(sKey))
            {
                sKey = DesKey;
            }
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            var inputByteArray = Encoding.Default.GetBytes(text);
            des.Key = Encoding.ASCII.GetBytes(GetMd5Hash(sKey).Substring(0, 8));
            des.IV = Encoding.ASCII.GetBytes(GetMd5Hash(sKey).Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(string text)
        {
            return !string.IsNullOrEmpty(text) ? Decrypt(text, DesKey) : "";
        }

        /// <summary> 
        /// 解密数据（对上面的加密方法进行解密） 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string text, string sKey)
        {
            if (string.IsNullOrWhiteSpace(sKey))
            {
                sKey = DesKey;
            }
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            var len = text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x;
            for (x = 0; x < len; x++)
            {
                var i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = Encoding.ASCII.GetBytes(GetMd5Hash(sKey).Substring(0, 8));
            des.IV = Encoding.ASCII.GetBytes(GetMd5Hash(sKey).Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }
            return sBuilder.ToString();
        }
        #endregion

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encoding">编码格式</param>
        /// <param name="json">加密的字符串</param>
        /// <returns></returns>
        public static string Base64Encrypt(Encoding encoding, string json)
        {
            string encodeString = "";
            byte[] b = encoding.GetBytes(json);
            try
            {
                encodeString = Convert.ToBase64String(b);
            }
            catch (Exception ex)
            {
                encodeString = json;
            }
            return encodeString;
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string BaseDecrypt(Encoding encoding, string json)
        {
            string decryptString = "";
            byte[] b = Convert.FromBase64String(json);
            try
            {
                decryptString = encoding.GetString(b);
            }
            catch (Exception ex)
            {
                decryptString = json;
            }
            return decryptString;

        }

        /// <summary>
        /// 进行按位运算
        /// </summary>
        /// <param name="abyte0"></param>
        /// <returns></returns>
        public static string byte2hex(byte[] abyte0)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < abyte0.Length; i++)
            {

                if ((abyte0[i] & 0xff) < 16)
                {
                    sb.Append("0");
                }
                sb.Append(Convert.ToString((long)abyte0[i] & (long)255, 16));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 对MD5的加密串进行Base64转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string MD5Base64(byte[] data)
        {
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(data);
            var str = Convert.ToBase64String(bs);
            return str;
        }

        /// <summary>
        /// 对进行MD5处理后的字符串进行16进制的按位运算，增加碰撞阻力
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string MD5Security(string json)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(json));
            return byte2hex(data);
        }
    }
}
