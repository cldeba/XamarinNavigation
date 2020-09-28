using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinNavigation.NavigationTypes;

namespace XamarinNavigation
{
    public abstract class NavigationMode
    {
        protected internal abstract Task Navigate(INavigation navigation, Page page);

        private static Lazy<ClearNavigationMode> _Clear = new Lazy<ClearNavigationMode>();
        private static Lazy<PushNavigationMode> _Push = new Lazy<PushNavigationMode>();

        public static NavigationMode Clear => _Clear.Value;
        public static NavigationMode Default => _Push.Value;
    }
}
