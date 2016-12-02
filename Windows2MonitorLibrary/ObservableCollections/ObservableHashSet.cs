using System.Collections.Generic;
using System.Collections.Specialized;

namespace Windows2MonitorLibrary.ObservableCollections
{
    public class ObservableHashSet<T> : HashSet<T>, INotifySetChanged<T>
    {
        public event NotifySetChangedEventHandler<T> NotifySetAddedElement;
        public event NotifySetChangedEventHandler<T> NotifySetRemovedElement;

        public ObservableHashSet() : base()
        {
        }

        /// <summary>
        /// Adds a new item to the set; notifies CollectionChanged
        /// </summary>
        /// <param name="item"></param>
        public new bool Add(T item)
        {
            NotifySetAddedElement?.Invoke(this, new NotifySetChangedEventArgs<T>(item));
            return base.Add(item);
        }

        public new bool Remove(T item)
        {
            NotifySetChangedEventArgs<T> oldItemArgs = new NotifySetChangedEventArgs<T>();
            oldItemArgs.OldItem = item;
            NotifySetRemovedElement?.Invoke(this, oldItemArgs);

            return base.Remove(item);
        }
    }
}
