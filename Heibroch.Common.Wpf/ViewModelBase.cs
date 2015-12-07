using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Heibroch.Common.Wpf
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected void RaisePropertyChanged<T>(Expression<Func<T>> expr)
        {
            var passedProperty = ((MemberExpression)expr.Body).Member.Name;
            RaisePropertyChanged(passedProperty);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
