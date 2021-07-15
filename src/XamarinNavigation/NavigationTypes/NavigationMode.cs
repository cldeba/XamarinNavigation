using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinNavigation.NavigationTypes;

namespace XamarinNavigation
{
    /// <summary>
    /// Represents the base class for navigation modes.
    /// </summary>
    public abstract class NavigationMode
    {
        /// <summary>
        /// Navigates to the specified page.
        /// </summary>
        /// <param name="navigation">The <see cref="INavigation"/> instance to be used for navigation.</param>
        /// <param name="page">The <see cref="Page"/> that should be navigated to.</param>
        /// <returns></returns>
        protected internal abstract Task Navigate(INavigation navigation, Page page);

        private static Lazy<ClearNavigationMode> _Clear = new Lazy<ClearNavigationMode>();
        private static Lazy<PushNavigationMode> _Push = new Lazy<PushNavigationMode>();

        /// <summary>
        /// Gets the <see cref="ClearNavigationMode"/> instance. This navigation mode clears the navigation stack before navigating.
        /// </summary>
        public static NavigationMode Clear => _Clear.Value;

        /// <summary>
        /// Gets the default <see cref="NavigationMode"/> instance. This navigation mode just pushes the page onto the navigation stack.
        /// </summary>
        public static NavigationMode Default => _Push.Value;
    }
}
