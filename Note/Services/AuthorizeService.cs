using System;
using DeviceId;
using System.IO;
using System.Management;
using Note.InkCanvasEx.SDK.Accredit;

namespace Note.Services
{
    internal class AuthorizeService
    {
        private string UserId = "";
        private string PublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqQJx+FYYtHSRVCnQzdEX5NmzH5qoPwmYR7LqFf56zEFbEv4LJCleXXnjfP74gdQf7JDxoXAPMOqtMv2lHWg1AVldnhf7kYRlj05K9Cq0o2z93Cn7/hu8UnLF+hAz+LBduTjqdWzN6IQIdFaLeYUlbdKEqTY/A5jh1mVfUgHW34PVaZFsUd4X4uEuf88CFhYImHnssG+xT7PfYgcwTjLEWTmouutLSyd6F1oeqlhSC3S/Pvy9JXbMAaiyf9DFNYctu50c7gN4TbIspDmnkVc8sEubc3wq7NJhRNUc/JU0g5a1G7/TJW84x0v+3guFL2VOpIdUKcvYd9m26JBBJeMQswIDAQAB";
        private string BrandName = "Sunia.HTREngine.Recognize";
        private string OfflineCert = "V1Q+S3aC12JflkHgLbf4OOcC0lZcLiT/F7soNc3ZUCJ5T883M0syVz74YpETByCaEXaboGx459+Rpv6tn5k8fhb8TuLfyvKZxmweP1ZijgklVWww5HeNKg65t7845ofkcZ/W176pOF67NN5HbdepubQfkCzZwcvfNYv/6azjNh5OsSMtxPBdUtBHYIuHXhEBOqig1NxsdF7QwG72XgWkH9e/XYo+o2H/L3S8MbDyxLjlggWSa8vLkoZBz5PzpMQYdDjGce0vWjUu/3bv/x778nyOp15e4mouOzOqAq8Ud65E7bbLm8/pxOLpoyhoBjrQOYn5K2iJIOx43fn7e7KK6UYAdyKB2lHoQI6H7dQCHzYeiwFKJcMtVUMWhHtYch9dEbMS0o0EW+/obd1bfCHcsp70bod23btjmYzrx0/IUCY8LwBK+mVMdkAKdRr8wP/V+DH4O23GQ50CDPGjkxcLFz0sMfJILhMBolu7Tpoem0kgGqjz3Pxhc3vb6vnk12wSVgKDp2vFOzMsEVG/BBHxYBCYQ8XC5YgEJkFU66gqZJXsg0DFAUjHO+Upg23wr30TnmWsBVyEkfrgV7hbcK8LSDC5anH7AuvFqwPSjOZNEQOXWVb2LkaAquqAf2DbjlVViF1ERLklbbZ/tg/728SmXJCnwrJcRRUCQQbbGe6cQW4QpwtAFfE2SS+Ze9IhY77/pBLWDRM/05TKbl3RBOIz4tS3Ux2UN+5w1LARK6Ur0WfK5gsrdwRHM/RkfNpklPOP0O2bywciGqCt54EoALSPOGi5f9cJbtxk4y1YLwVbj1k2g4IyzkCBG44Vbf4FbvECwjttq5t0L5TG2QhigMF9eV15u3DpkBimtJIdJ8Dv9wcEuiSrLP5YztwPe4PB9G0E0HtihgPzmpr1JGeSKuZTgcINHuJhpco/8FmBjQGd6SJuQ2lQ9FBU/w+GBWGGBYV47Y5CDVKFgpjDAQGbq12SAjJeXIgXVWKTqc/Ehvl/+zOihwLecOtGj/EE6HnxiCQAVJM16J85Lr9ZSA/fxbvRPBCOmnHDf+LO1PgwrYPBy+9xyVWlPdhDuV8NTfuaOlPTPl5qaw+bGdFVTQbh+JsCOMc2MU/8iyoET7aXva12ycjCo5pUGUOxIx0fB1j/JYKK2FFjCPY4buifXNbJckdxmZQNwVXwRrDOMHcLU326+45RwAq0wB8PWtKsGqbppfmuL/Ha0H3jD3wl+Q4QcO9YZjy7SV3A4mdIAh1S9k/MQaM0YX8RfJk8hwL5fIbvMDRMKoFd7RfDnAIlog+i9eJIDKozPWqShlttrBAARr/PiUpGnJOcdL3R26qa4+IsypLydxYH0F6pBKwwJhdk+4wtfQ==";
        private string AppId = "20220415173641715";
        private string BaseUrl = "https://developer-cn.sunia.com/";
        private string NetworkVer = "v7";
        private string CertPath = "cer_extranet.p12";
        private string CertPwd = "sunia123!@#";
        private string OnlineCertPath = "appid_cert.txt";
        private string OfflineIdPath = "appid_offlineid.txt" ;
        private string RecFisrtUsePath = "appid_firstuse.txt";
        private Tuple<string, string> GetBoardInfo()
        {
            string model = string.Empty;
            string manufacturer = string.Empty;
            try
            {
                var query = "SELECT * FROM Win32_ComputerSystem";
                var sercher = new ManagementObjectSearcher(query);
                var data = sercher.Get().GetEnumerator();
                data.MoveNext();
                ManagementBaseObject managementObject = data.Current;
                manufacturer = managementObject.GetPropertyValue("Manufacturer").ToString();
                model = managementObject["Model"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new Tuple<string, string>(manufacturer, model);
        }

        public static string GetMachineUuid()
        {
            string deviceId = new DeviceIdBuilder()
                    .AddMachineName()
                    .AddMacAddress()
                    .OnWindows(windows => windows.AddMachineGuid()
                                                .AddMotherboardSerialNumber()
                                                .AddProcessorId()
                                                .AddSystemDriveSerialNumber()
                                                .AddSystemUuid())
                    .ToString();
            return deviceId;
        }
        public VerifyInfo CreateVerifyInfo()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Verify");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Console.WriteLine("Verify dir:{0}", path);
                var boardInfo = GetBoardInfo();
                var verifyInfo = new VerifyInfo();
                //授权信息
                verifyInfo.AppID = this.AppId;
                verifyInfo.PublicKey = this.PublicKey;
                verifyInfo.BrandName = this.BrandName;
                verifyInfo.OfflineCert = this.OfflineCert;
                verifyInfo.Model = boardInfo.Item2;
                verifyInfo.Manufacturer = boardInfo.Item1;
                verifyInfo.DeviceId = GetMachineUuid();
                verifyInfo.UserId = this.UserId;
                //网络信息
                verifyInfo.BaseUrl = this.BaseUrl;
                verifyInfo.CertPwd = this.CertPwd;
                verifyInfo.CertPath = Path.Combine(Environment.CurrentDirectory, this.CertPath);
                verifyInfo.NetworkVer = this.NetworkVer;
                //保存路径
                verifyInfo.OnlineCertPath = Path.Combine(path, this.OnlineCertPath);
                verifyInfo.RecFisrtUsePath = Path.Combine(path, this.RecFisrtUsePath);
                verifyInfo.OfflineIdPath = Path.Combine(path, this.OfflineIdPath);
                Console.WriteLine("GUID:{0}, Manufacturer:{1}, Model:{2}", verifyInfo.DeviceId, verifyInfo.Manufacturer, verifyInfo.Model);
                return verifyInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
    }
}
