using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace XamarinNavigation
{
    /// <summary>
    /// Represents the base class for ViewModels
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when the current ViewModel was navigated to.
        /// </summary>
        public event EventHandler Navigated;

        /// <summary>
        /// Gets the <see cref="INavigationService"/> instance.
        /// </summary>
        protected internal INavigationService NavigationService { get; internal set; }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="property"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        internal Task OnNavigatedInternal()
        {
            Task result = OnNavigated();
            Navigated?.Invoke(this, EventArgs.Empty);
            return result;
        }

        /// <summary>
        /// This method is called when the current ViewModel was navigated to.
        /// </summary>
        /// <returns></returns>
        protected internal virtual Task OnNavigated()
        {
            return Task.CompletedTask;
        }
    }
}