using System.Collections.Generic;

namespace Born.Framework.Events
{
    public class XEvent<T>
    {
        private readonly List<IXEventListener<T>> eventListeners = new List<IXEventListener<T>>();
        private XEvent<T> instance;
        public XEvent<T> Instance => instance ??= new XEvent<T>();

        public void Raise(T item)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(item);
        }

        public void RegisterListener(IXEventListener<T> listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(IXEventListener<T> listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }

    public interface IXEventListener<T>
    {
        void OnEventRaised<TU>(TU item);
    }
}