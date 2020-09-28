using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace XamarinNavigation.Unity
{
    public static class NavigationServiceExtensions
    {
        /// <summary>
        /// Sets both the default view activator and ViewModel activator to the specified Unity container.
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="unityContainer">The Unity container to be used for view and ViewModel activation.</param>
        public static void UseUnityContainer(this INavigationService navigationService, UnityContainer unityContainer)
        {
            UnityActivator activator = new UnityActivator(unityContainer);
            navigationService.DefaultViewActivator = activator;
            navigationService.DefaultViewModelActivator = activator;
        }

        /// <summary>
        /// Sets the default view activator to the specified Unity container.
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="unityContainer">The Unity container to be used for view activation.</param>
        public static void UseUnityContainerForViewResolving(this INavigationService navigationService, UnityContainer unityContainer)
        {
            navigationService.DefaultViewActivator = new UnityActivator(unityContainer);
        }

        /// <summary>
        /// Sets the default ViewModel activator to the specified Unity container.
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="unityContainer">The Unity container to be used for ViewModel activation.</param>
        public static void UseUnityContainerForViewModelResolving(this INavigationService navigationService, UnityContainer unityContainer)
        {
            navigationService.DefaultViewModelActivator = new UnityActivator(unityContainer);
        }
    }
}
