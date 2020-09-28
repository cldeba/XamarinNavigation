using System;
using System.Collections.Generic;
using System.Text;
using XamarinNavigation.NavigationTypes;

namespace XamarinNavigation.Playground.ViewModels
{
    public class Page2ViewModel : ViewModelBase
    {
        public void NavigateToPage3()
        {
            NavigationService.Navigate<Page3ViewModel>(navigationType: new ClearNavigationMode());
        }
    }
}
