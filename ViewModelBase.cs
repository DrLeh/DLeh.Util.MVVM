using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

using System.Windows.Media;
using System.Collections;
using System.Data;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace DLeh.Util.MVVM
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {

        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string info = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
        protected virtual void NotifyPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        protected void NotifyPropertyChanged<T>(params Expression<Func<T>>[] expression)
        {
            foreach (var exp in expression)
                NotifyPropertyChanged(((MemberExpression)exp.Body).Member.Name);
        }

        protected virtual void ForwardPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged(sender, args);
        }

        /// <summary>
        /// Checks if the view is currently in design mode
        /// </summary>
        public bool IsInDesignMode
        {
            get
            {
                return DesignerProperties.GetIsInDesignMode(new DependencyObject());
            }
        }
    }
}
