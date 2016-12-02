using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows2MonitorLibrary.ObservableCollections
{
    public delegate void NotifySetChangedEventHandler<T>(object sender, NotifySetChangedEventArgs<T> e);

    public class NotifySetChangedEventArgs<T>
    {
        public NotifySetChangedEventArgs() { }

        public NotifySetChangedEventArgs(T newItem)
        {
            NewItem = newItem;
        }

        public T NewItem { get; set; }
        public T OldItem { get; set; }
    }

    public interface INotifySetChanged<T>
    {
        event NotifySetChangedEventHandler<T> NotifySetAddedElement;
        event NotifySetChangedEventHandler<T> NotifySetRemovedElement;
    }   
}
