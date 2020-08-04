using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinNavigation
{
    internal class ViewToViewModelFactory
    {
        public ViewToViewModelFactory(Type viewType, Type viewModelType, Func<VisualElement> viewCreator, Func<ViewModelBase> viewModelCreator, NavigationType defaultNavigationType)
        => (ViewType, ViewModelType, ViewCreator, ViewModelCreator, DefaultNavigationType) = (viewType, viewModelType, viewCreator, viewModelCreator, defaultNavigationType);

        public Type ViewType { get; }
        public Type ViewModelType { get; }
        public Func<VisualElement> ViewCreator { get; }
        public Func<ViewModelBase> ViewModelCreator { get; }
        public bool IsMainViewModel { get; set; }
        public NavigationType DefaultNavigationType { get; set; }
    }
}
