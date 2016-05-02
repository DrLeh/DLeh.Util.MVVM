using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DLeh.Util.MVVM
{
    /// <summary>
    /// Wraps an item to provide an IsSelected and AltIsSelected booleans
    /// </summary>
    /// <typeparam name="T">Item to wrap</typeparam>
    public class Selectable<T> : INotifyPropertyChanged
    {
        // private bool _selectionStatus;

        public T Item { get; private set; }
        private bool isChecked;
        /// <summary>
        /// Denotes whether the object is selected or not
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; NotifyPropertyChanged(); }
        }

        private bool isSelected;
        /// <summary>
        /// Provides an alternate field to bind to. useful in datagrids
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; NotifyPropertyChanged(); }
        }

        public Selectable(T item)
            : this(item, false) { }
        public Selectable(T item, bool selectionStatus)
        {
            Item = item;
            IsChecked = selectionStatus;
            IsSelected = selectionStatus;
        }

        public static implicit operator Selectable<T>(T item)
        {
            return new Selectable<T>(item, false);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName]string info = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
        //protected virtual void NotifyPropertyChanged(string info = null)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(info));
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj is Selectable<T>)
        //    {
        //        return Item.Equals(((Selectable<T>)obj).Item);
        //    }
        //    return base.Equals(obj);
        //}
    }

    public static class SelectableExtensions
    {
        /// <summary>
        /// Returns a <see cref="IEnumerable{T}" /> of <see cref="Selectable{T}"/>
        /// </summary>
        public static IEnumerable<Selectable<TSource>> AsSelectable<TSource>(this IEnumerable<TSource> source, bool selectionStatus = false)
        {
            if (source == null) yield break;
            foreach (var item in source)
                yield return new Selectable<TSource>(item, selectionStatus);
        }
    }
}
