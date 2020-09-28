using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinNavigation.Pages
{
    public class TabbedPage<TViewModel> : TabbedPage, IPage<TViewModel>
        where TViewModel : ViewModelBase
    {
        public TViewModel ViewModel
        {
            get => BindingContext as TViewModel;
            set => BindingContext = value;
        }

        protected INavigationService NavigationService => ViewModel?.NavigationService;
    }
}
