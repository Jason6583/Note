using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace Note.Services.YouDao
{
    public class YouDaoApiService
    {
        const string _appKey = "2a53d597c78b26de";
        const string _from = "auto";
        const string _to = "auto";
        const string _appSecret = "h8qKbfVNizzO4MzFt9xsM2iVQS0HHa9W";
        public static async Task<YouDaoTranslationData> GetWordsAsync(string queryText)
        {
            var requestUrl = GetRequestUrl(queryText);
            WebRequest translationWebRequest = WebRequest.Create(requestUrl);
            var response = await translationWebRequest.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream ?? throw new InvalidOperationException(), Encoding.GetEncoding("utf-8")))
                {
                    string result = reader.ReadToEnd();
                    var youDaoTranslationResponse = JsonConvert.DeserializeObject<YouDaoTranslationResponse>(result);
                    return new YouDaoTranslationData { ResultDetail = result, YouDaoTranslation = youDaoTranslationResponse };
                }
            }
        }
        private static string GetRequestUrl(string queryText)
        {
            string salt = DateTime.Now.Millisecond.ToString();
            MD5 md5 = new MD5CryptoServiceProvider();
            string md5Str = _appKey + queryText + salt + _appSecret;
            byte[] output = md5.ComputeHash(Encoding.UTF8.GetBytes(md5Str));
            string sign = BitConverter.ToString(output).Replace("-", "");
            var requestUrl = string.Format(
                "http://openapi.youdao.com/api?appKey={0}&q={1}&from={2}&to={3}&sign={4}&salt={5}",
                _appKey,
                HttpUtility.UrlDecode(queryText, System.Text.Encoding.GetEncoding("UTF-8")),
                _from, _to, sign, salt);
            return requestUrl;
        }
    }
    public class YouDaoTranslationData
    {
        public YouDaoTranslationResponse YouDaoTranslation { get; set; }
        public string ResultDetail { get; set; }
    }
}
