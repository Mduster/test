using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace test
{
    public class SQLHelper
    {
        public static string key = "abcdefgh";
        /// <summary>
        /// 数据库插入，增加，删除，返回int类型更改行数。
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string SQL, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ToString()))
            {
                conn.Open();
                using (SqlCommand dosql = conn.CreateCommand())
                {
                    dosql.CommandText = SQL;
                    dosql.Parameters.AddRange(parameters);
                    return dosql.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 数据库信息查找，返回当前查找的一行一列的object类型数据。
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string SQL, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ToString()))
            {
                conn.Open();
                using (SqlCommand dosql = conn.CreateCommand())
                {
                    dosql.CommandText = SQL;
                    dosql.Parameters.AddRange(parameters);
                    return dosql.ExecuteScalar();
                }
            }
        }
        /// <summary>
        /// 数据库中信息查找，返回查找的DataTable表中数据。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset);
                    return dataset.Tables[0];
                }
            }
        }
        /// <summary>
        /// 加密函数,秘钥仅支持8字节字符串。
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Encrypt(string pToEncrypt)
        {
            //访问数据加密标准(DES)算法的加密服务提供程序 (CSP) 版本的包装对象  
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ASCIIEncoding.ASCII.GetBytes("abcdefgh");　//建立加密对象的密钥和偏移量  
            des.IV = ASCIIEncoding.ASCII.GetBytes("abcdefgh");　 //原文使用ASCIIEncoding.ASCII方法的GetBytes方法  

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);//把字符串放到byte数组中    
            MemoryStream ms = new MemoryStream();//创建其支持存储区为内存的流　  
            //定义将数据流链接到加密转换的流  
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();              //上面已经完成了把加密后的结果放到内存中去  

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }
        /// <summary>
        /// 解密密码，返回字符串类型的密码
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <returns></returns>
        public static string Decrypt(string pToDecrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes("abcdefgh");　//建立加密对象的密钥和偏移量，此值重要，不能修改  
            des.IV = ASCIIEncoding.ASCII.GetBytes("abcdefgh");
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //建立StringBuild对象，createDecrypt使用的是流对象，必须把解密后的文本变成流对象  
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
    }
}