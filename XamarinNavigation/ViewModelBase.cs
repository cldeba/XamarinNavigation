using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XamarinNavigation
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected internal INavigationService NavigationService { get; internal set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected internal virtual void OnNavigated()
        {

        }
    }
}