namespace Note.InkCanvasEx.Events
{
    public class EventBusConfiguration : IEventBusConfiguration
    {
        public bool ThrowSubscriberException { get; set; } = false;

        internal static EventBusConfiguration Default = new EventBusConfiguration();
    }
}
