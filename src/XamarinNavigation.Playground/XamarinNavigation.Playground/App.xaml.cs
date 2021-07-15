using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinNavigation.Playground.ViewModels;

namespace XamarinNavigation.Playground
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();

            NavigationService navigationService = new NavigationService();
            navigationService
                .RegisterViewModel<MainPage, MainViewModel>()
                .RegisterViewModel<Page2, Page2ViewModel>()
                .RegisterViewModel<Page3, Page3ViewModel>()
                .SetMainViewModel<MainViewModel>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
