using Note.InkCanvasEx.SDK.Native;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Note.InkCanvasEx.SDK.Accredit
{
    /// <summary>
    /// Description:
    /// Author: EInkNote
    /// CreateDate: 2022/4/21 16:05:57
    /// Version:
    /// </summary>
    public class VerifyModule
    {
        /// <summary>授权回调</summary>
        public Action<int, string> VerifyCallback;
        /// <summary>授权信息</summary>
        private VerifyInfo _verifyInfo;
        private bool _isVerify = true;

        /// <summary>是否验证通过</summary>
        public bool IsVerify => this._isVerify;
        public VerifyModule(VerifyInfo verifyInfo) 
            => this._verifyInfo = verifyInfo != null 
            ? verifyInfo 
            : throw new ArgumentNullException(nameof(verifyInfo));
        public void VerifyAsync() => Task.Factory.StartNew((Action)(() =>
        {
            try
            {
                this.Verify();
            }
            catch (Exception ex)
            {
                Action<int, string> verifyCallback = this.VerifyCallback;
                if (verifyCallback == null)
                    return;
                verifyCallback(4096, ex.Message + "\r\n" + ex.StackTrace);
            }
        }));
        private void Verify()
        {
            IntPtr verify = LocalVerifyNative.CreateVerify();
            if (verify == IntPtr.Zero)
            {
                Action<int, string> verifyCallback = this.VerifyCallback;
                if (verifyCallback == null)
                    return;
                verifyCallback(4097, "Create verify object failed");
            }
            else
            {
                int code1 = LocalVerifyNative.SetVerifyInfo(verify, this._verifyInfo.AppID, this._verifyInfo.PublicKey, this._verifyInfo.DeviceId, this._verifyInfo.Model, this._verifyInfo.Manufacturer, this._verifyInfo.UserId, this._verifyInfo.BrandName, this._verifyInfo.OfflineCert);
                if (code1 != 0)
                {
                    this.OnVerifyCallback(code1, "SetVerifyInfo failed");
                }
                else
                {
                    int code2 = LocalVerifyNative.SetNetInfo(verify, this._verifyInfo.BaseUrl, this._verifyInfo.CertPwd, this._verifyInfo.CertPath, this._verifyInfo.NetworkVer);
                    if (code2 != 0)
                    {
                        this.OnVerifyCallback(code2, "SetNetInfo failed");
                    }
                    else
                    {
                        int code3 = LocalVerifyNative.SetSavePath(verify, this._verifyInfo.OnlineCertPath, this._verifyInfo.RecFisrtUsePath, this._verifyInfo.OfflineIdPath);
                        if (code3 != 0)
                        {
                            this.OnVerifyCallback(code3, "SetSavePath failed");
                        }
                        else
                        {
                            int code4 = LocalVerifyNative.RegErrorMsg(verify, new InfoCallback(this.RegErrorMsgCallback));
                            if (code4 != 0)
                            {
                                this.OnVerifyCallback(code4, "RegErrorMsg failed");
                            }
                            else
                            {
                                int code5 = LocalVerifyNative.RegOutputMsg(verify, new InfoCallback(this.RegOutputMsgCallBack));
                                if (code5 != 0)
                                {
                                    this.OnVerifyCallback(code5, "RegOutputMsg failed");
                                }
                                else
                                {
                                    int code6 = LocalVerifyNative.Verify(verify);
                                    if (code6 != 0)
                                    {
                                        this.OnVerifyCallback(code6, "Verify failed");
                                    }
                                    else
                                    {
                                        Action<int, string> verifyCallback = this.VerifyCallback;
                                        if (verifyCallback == null)
                                            return;
                                        verifyCallback(0, "Verify successed");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RegErrorMsgCallback(string error)
        {
            string str = Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(error));
            Action<int, string> verifyCallback = this.VerifyCallback;
            if (verifyCallback == null)
                return;
            verifyCallback(16384, str);
        }

        private void RegOutputMsgCallBack(string msg)
        {
            string str = Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(msg));
            Action<int, string> verifyCallback = this.VerifyCallback;
            if (verifyCallback == null)
                return;
            verifyCallback(16385, str);
        }

        private void OnVerifyCallback(int code, string message)
        {
            if (code != 0)
                this._isVerify = false;
            message = string.Format("Verify error:{0}, message:{1}", (object)(VerifyError)code, (object)message);
            Action<int, string> verifyCallback = this.VerifyCallback;
            if (verifyCallback == null)
                return;
            verifyCallback(code, message);
        }
    }
}
