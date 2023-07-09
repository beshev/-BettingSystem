namespace Infrastructure.Events
{
    public class EventSubscriber : IEventSubscriber
    {
        public EventSubscriber(IEventPublisher publisher)
        {
            publisher.ChangeEvent += HandleEventForChange;
            publisher.HideEvent += HandleEventForHide;
        }

        private void HandleEventForChange(string eventData)
        {
            // Send the new data somewhere :)
        }

        private void HandleEventForHide(int id)
        {
            // Send the id of the element that need to be hide
        }
    }
}
