using Note.InkCanvasEx.Commons;
using System.Collections.Generic;

namespace Note.Models
{
    public class CandidateItem : BindableBase
    {
        private int wordId;
        /// <summary>
        /// 识别结果
        /// </summary>
        public int WordId
        {
            get => this.wordId;
            set => this.SetProperty(ref this.wordId, value);
        }
        private string word;
        /// <summary>
        /// 识别结果
        /// </summary>
        public string Word
        {
            get => this.word;
            set => this.SetProperty(ref this.word, value);
        }

        private List<string> candidates = new List<string>();
        /// <summary>
        /// 候选字列表
        /// </summary>
        public List<string> Candidates
        {
            get => this.candidates;
            set => this.SetProperty(ref this.candidates, value);
        }
    }
}
