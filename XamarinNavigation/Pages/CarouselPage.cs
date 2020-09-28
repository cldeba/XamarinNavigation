using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinNavigation.Pages
{
    public class CarouselPage<TViewModel> : CarouselPage, IPage<TViewModel>
        where TViewModel : ViewModelBase
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        public TViewModel ViewModel
        {
            get => BindingContext as TViewModel;
            set => BindingContext = value;
        }

        protected INavigationService NavigationService => ViewModel?.NavigationService;
    }
}
