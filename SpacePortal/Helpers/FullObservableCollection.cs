using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacePortal.Core.Models;
using Windows.Foundation.Collections;

namespace SpacePortal.Helpers
{
    public class FullObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        private ObservableCollection<InformationsForGradesPage_GradesRow> gradesRows;

        public FullObservableCollection()
            : base()
        {
            CollectionChanged += new NotifyCollectionChangedEventHandler(FullObservableCollection_CollectionChanged);
        }
        public FullObservableCollection(ObservableCollection<T> list)
            : base(list)
        {
            foreach (var item in list)
            {
                item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
            }
            CollectionChanged += new NotifyCollectionChangedEventHandler(FullObservableCollection_CollectionChanged);
        }

        void FullObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Object item in e.NewItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
            if (e.OldItems != null)
            {
                foreach (Object item in e.OldItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged -= new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyCollectionChangedEventArgs a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(a);
        }
    }
}
