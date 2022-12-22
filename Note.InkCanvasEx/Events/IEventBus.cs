using System;

namespace Note.InkCanvasEx.Events
{
    public interface IEventBus
    {
        SubscriptionToken Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase;
        void Unsubscribe(SubscriptionToken token);
        void Publish<TEventBase>(TEventBase eventItem) where TEventBase : EventBase;
        void PublishAsync<TEventBase>(TEventBase eventItem) where TEventBase : EventBase;
        void PublishAsync<TEventBase>(TEventBase eventItem, AsyncCallback callback) where TEventBase : EventBase;
    }
}
