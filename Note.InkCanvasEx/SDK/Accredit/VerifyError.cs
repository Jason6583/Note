namespace Note.InkCanvasEx.SDK.Accredit
{
    /// <summary>
    /// Description:
    /// Author: EInkNote
    /// CreateDate: 2022/4/2 15:19:00
    /// Version:
    /// </summary>
    public enum VerifyError
    {
        /// <summary>成功</summary>
        ERROR_OK,
        /// <summary>参数异常</summary>
        ERROR_PARAM_ILLEGAL,
        /// <summary>prepare数据异常</summary>
        ERROR_PREPARE_INPUTDATA,
        /// <summary>prepare连接异常</summary>
        ERROR_PREPARE_CNT,
        /// <summary>commit连接异常</summary>
        ERROR_COMMIT_CNT,
        /// <summary>prepare返回数据异常  5</summary>
        ERROR_PREPARE_RETDATA,
        /// <summary>不存在在线证书</summary>
        ERROR_ONLINECERT,
        /// <summary>不存在离线证书</summary>
        ERROR_OFFLINECERT,
        /// <summary>离线授权失败</summary>
        ERROR_VERIFYOFFLINE,
        /// <summary>commit返回数据异常</summary>
        ERROR_COMMIT_RETDATA,
        /// <summary>过期 10</summary>
        ERROR_VALIDITYTIME,
        /// <summary>授权失败</summary>
        ERROR_VERIFY,
        /// <summary>模块授权失败</summary>
        ERROR_MODULEVERIFY,
        /// <summary>豁免期结束</summary>
        ERROR_FREEUSE,
    }
}
