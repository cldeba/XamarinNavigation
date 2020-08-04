using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinNavigation.NavigationTypes;

namespace XamarinNavigation
{
    public abstract class NavigationType
    {
        protected internal abstract Task Navigate(INavigation navigation, Page page);

        private static Lazy<ClearNavigationType> _Clear = new Lazy<ClearNavigationType>();
        private static Lazy<PushNavigationType> _Push = new Lazy<PushNavigationType>();

        public static NavigationType Clear => _Clear.Value;
        public static NavigationType Default => _Push.Value;
    }
}
