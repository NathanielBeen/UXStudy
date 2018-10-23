using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    //this is a class I include in most projects I do. It is a wrapper for the INotifyPropertyChanged interface, which
    //allows properties in the view to dynamically respond to changes in properties in the viewmodel
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //a generic function to set any property that needs to fire PropertyChanged.
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            //return if the value equals the old value
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            //set the value and fire property changed
            storage = value;
            onPropertyChanged(propertyName);
            return true;
        }

        //invoke propertychanged with the given property name (allows view to update properly)
        protected void onPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
}
