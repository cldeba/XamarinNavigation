using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinNavigation
{
    /// <summary>
    /// Represents a <see cref="ICommand"/> implementation that can be used to navigate to the specified ViewModel type.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public class NavigateCommand<TViewModel> : ICommand
        where TViewModel : ViewModelBase
    {
        public event EventHandler CanExecuteChanged;

        public NavigateCommand(INavigationService navigationService, Func<object, bool> canExecuteDelegate = null)
        {
            NavigationService = navigationService;
            CanExecuteDelegate = canExecuteDelegate;
        }

        private INavigationService NavigationService { get; }
        private Func<object,bool> CanExecuteDelegate { get; }

        public bool CanExecute(object parameter)
        {
            return CanExecuteDelegate?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            NavigationService?.Navigate<TViewModel>();
        }
    }
}
