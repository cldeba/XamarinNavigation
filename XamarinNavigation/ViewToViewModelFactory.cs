using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinNavigation
{
    internal class ViewToViewModelFactory
    {
        public ViewToViewModelFactory(Type viewType, Type viewModelType, Func<VisualElement> viewCreator, Func<ViewModelBase> viewModelCreator)
        => (ViewType, ViewModelType, ViewCreator, ViewModelCreator) = (viewType, viewModelType, viewCreator, viewModelCreator);

        public Type ViewType { get; }
        public Type ViewModelType { get; }
        public Func<VisualElement> ViewCreator { get; }
        public Func<ViewModelBase> ViewModelCreator { get; }
    }
}
