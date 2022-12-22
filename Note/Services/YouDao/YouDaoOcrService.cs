using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Note.Services.YouDao
{
    public class YouDaoOcrService
    {
        private static string appKey = "2a53d597c78b26de";
        private static string appSecret = "h8qKbfVNizzO4MzFt9xsM2iVQS0HHa9W";
        private static string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
        private static string Truncate(string q)
        {
            if (q == null)
            {
                return null;
            }
            int len = q.Length;
            return len <= 20 ? q : (q.Substring(0, 10) + len + q.Substring(len - 10, 10));
        }
        public static string LoadAsBase64(string filename)
        {
            try
            {
                FileStream filestream = new FileStream(filename, FileMode.Open);
                byte[] arr = new byte[filestream.Length];
                filestream.Position = 0;
                filestream.Read(arr, 0, (int)filestream.Length);
                filestream.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        protected static string Post(string url, Dictionary<String, String> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        public static Task<OcrResponceData> GetOcrAsync(string base64)
        {
            Dictionary<String, String> dic = new Dictionary<String, String>();
            string url = "https://openapi.youdao.com/ocrapi";
            string q = base64;
            string salt = DateTime.Now.Millisecond.ToString();
            string detectType = "10012";
            string imageType = "1";
            string langType = "zh-CHS";
            dic.Add("detectType", detectType);
            dic.Add("imageType", imageType);
            dic.Add("langType", langType);
            dic.Add("docType", "json");
            dic.Add("signType", "v3");
            dic.Add("img", HttpUtility.UrlEncode(q));
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            long millis = (long)ts.TotalMilliseconds;
            string curtime = Convert.ToString(millis / 1000);
            dic.Add("curtime", curtime);
            string signStr = appKey + Truncate(q) + salt + curtime + appSecret; ;
            string sign = ComputeHash(signStr, new SHA256CryptoServiceProvider());
            dic.Add("appKey", appKey);
            dic.Add("salt", salt);
            dic.Add("sign", sign);
            string result = Post(url, dic);
            var responce = JsonConvert.DeserializeObject<OcrResponceData>(result);
            return Task.FromResult(responce);
        }
    }
}
