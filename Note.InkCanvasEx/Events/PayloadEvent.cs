namespace Note.InkCanvasEx.Events
{
    public class PayloadEvent<TPayload> : EventBase
    {
        public TPayload Payload { get; protected set; }
        public PayloadEvent(TPayload payload)
        {
            Payload = payload;
        }
    }
}
