using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinNavigation.NavigationTypes
{
    /// <summary>
    /// The navigated page is pushed onto the navigation stack.
    /// </summary>
    public class PushNavigationType : NavigationType
    {
        protected internal override async Task Navigate(INavigation navigation, Page page)
        {
            await navigation.PushAsync(page);
        }
    }
}
