using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinNavigation.NavigationTypes
{
    /// <summary>
    /// The navigation stack is cleared before the navigated page is pushed.
    /// </summary>
    public class ClearNavigationMode : NavigationMode
    {
        protected internal override async Task Navigate(INavigation navigation, Page page)
        {
            navigation.InsertPageBefore(page, navigation.NavigationStack[0]);
            await navigation.PopToRootAsync();
        }
    }
}
