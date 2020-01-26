using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace XamarinNavigation.Unity
{
    public static class NavigationServiceExtensions
    {
        public static void UseUnityContainer(this INavigationService navigationService, UnityContainer unityContainer)
        {
            UnityActivator activator = new UnityActivator(unityContainer);
            navigationService.DefaultViewActivator = activator;
            navigationService.DefaultViewModelActivator = activator;
        }

        public static void UseUnityContainerForViewResolving(this INavigationService navigationService, UnityContainer unityContainer)
        {
            navigationService.DefaultViewActivator = new UnityActivator(unityContainer);
        }

        public static void UseUnityContainerForViewModelResolving(this INavigationService navigationService, UnityContainer unityContainer)
        {
            navigationService.DefaultViewModelActivator = new UnityActivator(unityContainer);
        }
    }
}
