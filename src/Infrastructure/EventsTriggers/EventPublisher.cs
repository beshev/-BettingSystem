namespace Infrastructure.Events
{
    using Newtonsoft.Json;

    public class EventPublisher : IEventPublisher
    {
        public delegate void EventHandlerChange(string eventData);

        public delegate void EventHandlerHide(IEnumerable<int> ids);

        public event EventHandlerChange ChangeEvent;

        public event EventHandlerHide HideEvent;

        public void TriggerEventForChanges<T>(IEnumerable<T> eventData)
        {
            if (ChangeEvent != null && eventData.Any())
            {
                var eventDataAsString = JsonConvert.SerializeObject(eventData);
                ChangeEvent.Invoke(eventDataAsString);
            }
        }

        public void TriggerEventForHide(IEnumerable<int> ids)
        {
            if (ChangeEvent != null && ids.Any())
            {
                HideEvent.Invoke(ids);
            }
        }
    }
}
