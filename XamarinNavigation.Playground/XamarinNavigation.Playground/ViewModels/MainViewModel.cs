using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinNavigation.Playground.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public void NavigateToPage2()
        {
            NavigationService.Navigate<Page2ViewModel>();
        }
    }
}
