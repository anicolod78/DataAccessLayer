using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IT.TnDigit.ORM.ClientController
{
    /// <summary>
    /// Descrizione di riepilogo per HelperEncrypt.
    /// riferimento : http://www.codeproject.com/aspnet/encrypt.asp
    /// </summary>
    public class HelperEncryptC
    {
        public HelperEncryptC()
        {
        }

        // Encrypt the text
        static public string EncryptText(string strText)
        {
            return Encrypt(strText, "&%#@?,:*");
        }

        //Decrypt the text 
        static public string DecryptText(string strText)
        {
            return Decrypt(strText, "&%#@?,:*");
        }

        //The function used to encrypt the text
        static private string Encrypt(string strText, string strEncrKey)
        {
            byte[] byKey = { };
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                string appEncrKey = strEncrKey;
                if (strEncrKey.Length > 8)
                {
                    appEncrKey = strEncrKey.Remove(8, strEncrKey.Length);
                }
                byKey = System.Text.Encoding.UTF8.GetBytes(appEncrKey);
                byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                return ex.Message;
            }
        }

        //The function used to decrypt the text
        static private string Decrypt(string strText, string sDecrKey)
        {
            if (strText == null)
                return "";

            byte[] byKey = { };
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] inputByteArray = new byte[strText.Length];
            try
            {
                string appEncrKey = sDecrKey;
                if (sDecrKey.Length > 8)
                {
                    appEncrKey = sDecrKey.Remove(8, sDecrKey.Length);
                }
                byKey = System.Text.Encoding.UTF8.GetBytes(appEncrKey);

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray(), 0, ms.ToArray().Length);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION - ERROR : " + ex.ToString());
                return strText;
            }

        }
    }
}
