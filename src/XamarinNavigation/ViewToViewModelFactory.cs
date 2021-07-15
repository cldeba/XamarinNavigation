using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinNavigation
{
    internal class ViewToViewModelFactory
    {
        public ViewToViewModelFactory(Type viewType, Type viewModelType, Func<VisualElement> viewCreator, Func<ViewModelBase> viewModelCreator, NavigationMode defaultNavigationMode)
        => (ViewType, ViewModelType, ViewCreator, ViewModelCreator, DefaultNavigationMode) = (viewType, viewModelType, viewCreator, viewModelCreator, defaultNavigationMode);

        public Type ViewType { get; }
        public Type ViewModelType { get; }
        public Func<VisualElement> ViewCreator { get; }
        public Func<ViewModelBase> ViewModelCreator { get; }
        public bool IsMainViewModel { get; set; }
        public NavigationMode DefaultNavigationMode { get; set; }
    }
}
