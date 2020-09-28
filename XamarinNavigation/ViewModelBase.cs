using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace XamarinNavigation
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Navigated;

        protected internal INavigationService NavigationService { get; internal set; }

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

        protected internal virtual Task OnNavigated()
        {
            return Task.CompletedTask;
        }
    }
}