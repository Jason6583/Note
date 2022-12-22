using Note.InkCanvasEx.SDK.Native;
using System;
using System.Runtime.InteropServices;

namespace Note.InkCanvasEx.SDK.Accredit
{
    /// <summary>
    /// Description: 授权接口
    /// Author: EInkNote
    /// CreateDate: 2022/4/2 15:08:47
    /// Version:
    /// </summary>
    public static class LocalVerifyNative
    {
        /// <summary>创建授权对象</summary>
        /// <returns>授权对象</returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern IntPtr CreateVerify();

        /// <summary>设置授权信息</summary>
        /// <param name="obj"></param>
        /// <param name="pstrAppID"></param>
        /// <param name="pstrPublicKey"></param>
        /// <param name="pstrDeviceId"></param>
        /// <param name="pstrModel"></param>
        /// <param name="pstrManufacturer"></param>
        /// <param name="pstrUserId"></param>
        /// <param name="pstrBrandName"></param>
        /// <param name="pstrOfflineCert"></param>
        /// <returns></returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int SetVerifyInfo(
          IntPtr obj,
          string pstrAppID,
          string pstrPublicKey,
          string pstrDeviceId,
          string pstrModel,
          string pstrManufacturer,
          string pstrUserId,
          string pstrBrandName,
          string pstrOfflineCert);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pstrBaseUrl"></param>
        /// <param name="pstrCertPwd"></param>
        /// <param name="pstrCertPath"></param>
        /// <param name="pstrNetworkVer"></param>
        /// <returns></returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int SetNetInfo(
          IntPtr obj,
          string pstrBaseUrl,
          string pstrCertPwd = null,
          string pstrCertPath = null,
          string pstrNetworkVer = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pstrOnlineCertPath"></param>
        /// <param name="pstrRecFisrtUsePath"></param>
        /// <param name="pstrOfflineIdPath"></param>
        /// <returns></returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int SetSavePath(
          IntPtr obj,
          string pstrOnlineCertPath,
          string pstrRecFisrtUsePath,
          string pstrOfflineIdPath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int RegModules(IntPtr obj, InfoCallback callback);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int VerifyOffline(IntPtr obj);

        /// <summary>设备授权状态</summary>
        /// <param name="obj">授权对象</param>
        /// <returns>0表示成功，其他表示失败 </returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int Verify(IntPtr obj);

        /// <summary>指定模块是否授权,需要在设备状态授权成功后调用</summary>
        /// <param name="obj">授权对象</param>
        /// <param name="pstrModuleName">模块名</param>
        /// <returns>0表示成功，其他表示失败</returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int VerifyModule(IntPtr obj, string pstrModuleName);

        /// <summary>注册返回授权异常信息回调</summary>
        /// <param name="obj">授权对象</param>
        /// <param name="callback"></param>
        /// <returns>0表示成功，其他表示失败</returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int RegErrorMsg(IntPtr obj, InfoCallback callback);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int RegOutputMsg(IntPtr obj, InfoCallback callback);

        /// <summary>注册返回Prepare接口参数回调</summary>
        /// <param name="obj">授权对象</param>
        /// <param name="callback"></param>
        /// <returns>0表示成功，其他表示失败</returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int RegPrepareParam(IntPtr obj, InfoCallback callback);

        /// <summary>注册返回Commit接口参数回调</summary>
        /// <param name="obj">授权对象</param>
        /// <param name="callback"></param>
        /// <returns>0表示成功，其他表示失败</returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int RegCommitParam(IntPtr obj, InfoCallback callback);

        /// <summary>注册获取Prepare返回数据回调</summary>
        /// <param name="obj">授权对象</param>
        /// <param name="callback"></param>
        /// <returns>0表示成功，其他表示失败</returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int RegPrepareRet(IntPtr obj, InfoCallback callback);

        /// <summary>注册获取Commit返回数据回调</summary>
        /// <param name="obj">授权对象</param>
        /// <param name="callback"></param>
        /// <returns>0表示成功，其他表示失败</returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int RegCommitRet(IntPtr obj, InfoCallback callback);

        /// <summary>注册获取在线证书明文数据回调</summary>
        /// <param name="obj">授权对象</param>
        /// <param name="callback"></param>
        /// <returns>0表示成功，其他表示失败</returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int RegOnlineCertPlainText(IntPtr obj, InfoCallback callback);

        /// <summary>注册获取离线证书明文数据回调</summary>
        /// <param name="obj">授权对象</param>
        /// <param name="callback">0表示成功，其他表示失败</param>
        /// <returns></returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int RegOfflineCertPlainText(IntPtr obj, InfoCallback callback);

        /// <summary>释放授权对象</summary>
        /// <param name="obj">授权对象</param>
        /// <returns>0表示成功，其他表示失败</returns>
        [DllImport("Dll\\verify.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int ReleaseVerify(IntPtr obj);
    }
}
