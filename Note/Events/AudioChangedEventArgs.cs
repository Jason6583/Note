using Note.InkCanvasEx.Events;

namespace Note.Events
{
    internal class AudioChangedEventArgs : EventBase
    {
        public string AudioPath { get; set; }
        public string AudioData { get; set; }
    }
}
