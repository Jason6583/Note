using System.Collections.Generic;

namespace Note.InkCanvasEx.SDK.Editor
{
    /// <summary>识别Editor，提供文字和公式识别功能</summary>
    public interface IRecognizeEditor
    {
        /// <summary>生成一个识别点</summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="t">点坐标产生的时间</param>
        /// <param name="action">点坐标类型</param>
        /// <returns></returns>
        IRecognizePoint ObtainPoint(float x, float y, long t, int action);
        /// <summary>批量添加设别点信息，每条笔划通过落笔点来分开</summary>
        /// <param name="recognizePoints">一批笔画包含的笔点信息</param>
        void AddPoints(List<IRecognizePoint> recognizePoints);
        /// <summary>增加一条笔划的笔点信息</summary>
        /// <param name="recognizePoints">一条笔画包含的笔点信息</param>
        /// <returns>返回笔迹的唯一ID</returns>
        int AddStrokePoints(List<IRecognizePoint> recognizePoints);
        /// <summary>通过笔划的ID,删除一条笔划</summary>
        /// <param name="strokeID">笔划的ID</param>
        /// <returns></returns>
        bool DeleteStrokePoints(int strokeID);
        /// <summary>
        /// 更新识别结果
        /// 当识别结果中包含words的时候，可以根据识别情况，用备选词代替返回的结果
        /// 此时可以把选择的备选词通过该方法传递回编辑器，以保证数据的一致性
        /// </summary>
        /// <param name="wordId">需要更新的词的Id</param>
        /// <param name="candidate">选择的备选词</param>
        void UpdateRecognizeWord(int wordId, string candidate);
        /// <summary>在整篇识别模式RecognitionMode.MODE_ECR下，调用该方法触发识别</summary>
        void DoPageRecognize();
        /// <summary>进行联想词匹配</summary>
        /// <param name="text">需要进行联想的词汇。联想结果在编辑器回调返回</param>
        void DoAssociate(string text);
    }
}
