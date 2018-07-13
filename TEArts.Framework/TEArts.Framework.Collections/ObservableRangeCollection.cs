using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEArts.Framework.Collections
{
    public class ObservableRangeCollection<T> : ObservableCollection<T>
    {
        public void AddRange(IList<T> range, int index = -1)
        {
            if (range == null || range.Count <= 0)
            {
                return;
            }
            int start = index;
            if (index < 0)
            {
                start = Count;
                index = Count;
            }
            foreach (T i in range)
            {
                Items.Insert(index, i);
            }

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveRange(IList<T> range)
        {
            if (range == null || range.Count <= 0)
            {
                return;
            }
            foreach (T i in range)
            {
                Items.Remove(i);
            }

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


    }
}
