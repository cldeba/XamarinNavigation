using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinNavigation
{
    internal class ViewToViewModelFactory
    {
        public ViewToViewModelFactory(Func<VisualElement> viewCreator, Func<ViewModelBase> viewModelCreator)
        => (ViewCreator, ViewModelCreator) = (viewCreator, viewModelCreator);

        public Func<VisualElement> ViewCreator { get; }
        public Func<ViewModelBase> ViewModelCreator { get; }
    }
}
