using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FinTracker.Utilities.Properties
{
    public interface IBindableObject : INotifyPropertyChanged
    {
        void RaisePropertyChanged(string propertyName = "");

        bool Set<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action? onChanged = null);
    }

    public abstract class BindableObject : IBindableObject//, ICloneable
    {
        /// <summary>
        /// Creates a new model that is a copy of the current model instance.
        /// </summary>        
        //public virtual object Clone()
        //{
        //    //SerializationHelper.Clone(this, out object clonedObject);
        //    return null;
        //}

        /// <summary>
        /// Raises the property changed event if the property has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Set<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null, Action? onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Occurs when a property value has changed.
        /// </summary>        
        public event PropertyChangedEventHandler? PropertyChanged;
    }

}
