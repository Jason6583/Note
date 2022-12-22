namespace Note.InkCanvasEx.Events
{
    public interface ISubscription
    {
        SubscriptionToken SubscriptionToken { get; }
        void Publish(EventBase eventBase);
    }
}
