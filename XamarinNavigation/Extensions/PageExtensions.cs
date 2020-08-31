using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinNavigation.Extensions
{
    internal static class PageExtensions
    {
        public static NavigationPage ToNavigationPage(this Page page)
        {
            return page is NavigationPage navigationPage ? navigationPage : new NavigationPage(page);
        }
    }
}
