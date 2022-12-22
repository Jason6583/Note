namespace Note.InkCanvasEx.SDK.Editor
{
    /// <summary>引擎editor，提供引擎基本的操作。</summary>
    public interface IBaseEditor
    {
        /// <summary>打开编辑器</summary>
        /// <param name="engineParams">编辑器的配置参数</param>
        /// <returns>成功返回 true</returns>
        bool Open(IParams engineParams);
        /// <summary>关闭编辑器</summary>
        void Close();
        /// <summary>更新编辑器配置参数</summary>
        /// <param name="engineParams">编辑器参数</param>
        /// <returns>成功返回 true</returns>
        bool UpdateParams(IParams engineParams);
        /// <summary>
        /// 获取编辑器配置参数
        /// 当编辑器已经打开，获取编辑器配置参数
        /// 否则生成一个新的配置参数
        /// </summary>
        /// <returns></returns>
        IParams ObtainParams();
        /// <summary>
        /// 判断编辑器是否空闲
        /// 预留接口，暂未实现
        /// </summary>
        /// <returns>空闲返回true， 否则false</returns>
        bool IsIdle();
        /// <summary>等待编辑器空闲</summary>
        void WaitForIdle();
        /// <summary>检查是否已是关闭状态</summary>
        /// <returns>如果已关闭，返回true</returns>
        bool IsClosed();
        /// <summary>检查内容是否为空</summary>
        /// <returns></returns>
        bool IsEmpty();
        /// <summary>获取当前已识别内容</summary>
        /// <returns>返回编辑器内的识别内容，json格式</returns>
        string GetContent();
        /// <summary>清除编辑器中的内容</summary>
        void ClearContent();
    }
}
