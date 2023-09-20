using Autofac;

namespace ChatApp.Infrastructure
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IComponentContext _componentContext;

        public EventDispatcher(
            IComponentContext componentContext
            )
        {
            _componentContext = componentContext;
        }

        public Task Dispatch<TEvent>(TEvent e) where TEvent : IEvent
        {
            if(e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var eventType = typeof(IEventHandler<>).MakeGenericType(e.GetType());

            dynamic handler = _componentContext.Resolve(eventType);

            return (Task)eventType
                .GetMethod("Handle")
                .Invoke(handler, new object[] { e });
        }
    }
}
