# XamarinNavigation

XamarinNavigation brings you all the functionality you need to implement clean page navigation in your Xamarin.Forms app while providing testability, loose coupling and MVVM compliance.

## Building your architecture

#### ViewModels

The use of `XamarinNavigation` requires that your MVVM Architecture uses the `ViewModelBase` class as base class for all navigatable ViewModels. The virtual method `OnNavigated` informs you when your ViewModel instance got navigated to:

```
using XamarinNavigation;

public class YourViewModel : ViewModelBase
{
    // Properties, ViewModel logic
    
    protected override async Task OnNavigated()
    {
    
    }
}
```

## Registering ViewModels

On startup of your app, register your ViewModels as follows:

```
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        
        ConfigureNavigation();
    }
    
    private void ConfigureNavigation()
    {
        NavigationService navigationService = new NavigationService();
        
        navigationService.RegisterViewModel<YourPage, YourViewModel>();
        navigationService.RegisterViewModel<YourOtherPage, YourOtherViewModel>();
    }
    
    // ...
}
```

Set the main page like this:

```
navigationService.RegisterViewModel<MainPage, MainViewModel>();
// ...
navigationService.SetMainViewModel<MainViewModel>();
```

## Navigating

Inside your ViewModel you can navigate to other pages using the protected `NavigationService` property.

```
NavigationService.Navigate<YourViewModel>();
```

Optionally you can **pass a ViewModel instance**:

```
YourViewModel viewModelInstance = new YourViewModel();
NavigationService.Navigate<YourViewModel>(viewModel: viewModelInstance);
```

Of course, you can **pass arguments** to your ViewModel instance using the `options` parameter:

```
NavigationService.Navigate<YourViewModel>(options: vm => vm.MyArgument = "Hello World");
```

In addition, you can also specify the **navigation mode**:

```
NavigationService.Navigate<YourViewModel>(navigationMode: NavigationMode.Push);
```

Out of the box, XamarinNavigation offers the following navigation modes:
 * **Push**: Simply pushes the page onto the Xamarin.Forms navigation stack
 * **Clear**: Clears the navigation Xamarin.Forms navigation stack before pushing the page onto it

It's easy to implement custom navigation modes by just inheriting from the abstract `NavigationMode` class:

```
public class CustomNavigationMode : NavigationMode
{
    protected internal override async Task Navigate(INavigation navigation, Page page)
    {
        // Your custom navigation logic
    }
}

// In your ViewModel:
NavigationService.Navigate<YourViewModel>(navigationMode: new CustomNavigationMode());

// Or, alternatively when registering your ViewModel:
CustomNavigationMode navigationMode = new CustomNavigationMode()
navigationService.RegisterViewModel<TestPage, TestViewModel>(defaultNavigationMode: navigationMode)

```

## Other features that might be of interest

#### ViewModel and View activation

For the creation of View and ViewModel instances, the `NavigationService` class provides the properties `DefaultViewActivator` and `DefaultViewModelActivator` which are both of type `IActivator`.

Implement your custom activator if you want to change the default activation mode:

```
public class DefaultActivator : IActivator
{
    public T GetInstance<T>()
    {
        ...
    }
}
```

Per default, activation is handled via reflection.

If you prefere to use a **Unity container** you can use the XamarinNavigation.Unity package:

```
using XamarinNavigation;
using XamarinNavigation.Unity;

// ...

navigationService.UseUnityContainer(yourUnityContainer);

```

Alternatively, you can use different containers for View and ViewModel resolving:

```
navigationService.UseUnityContainerForViewResolving(yourUnityContainer);
navigationService.UseUnityContainerForViewModelResolving(yourOtherUnityContainer);
```

#### Pages

You can use the normal Page base classes provided by Xamarin.Forms like `ContentPage`, `MasterDetailPage` etc.
If in your scenario it might be important to access the ViewModel functionality from your page code-behind, you can use the generic `...Page<TViewModel>` classes provided by XamarinNavigation for convenience purposes:
 * `ContentPage<TViewModel>`
 * `MasterDetailPage<TViewModel>`
 * `CarouselPage<TViewModel>`
 * `TabbedPage<TViewModel>`
 
They expose the ViewModel instance through their protected `ViewModel` property.

Use the pages like this:

```
<pages:ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
                   xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                   xmlns:pages="clr-namespace:XamarinNavigation.Pages"
                   xmlns:viewModels="clr-namespace:YourApp.ViewModels"
                   x:Class="MobileReadingTest.Pages.MenuPage"
                   x:TypeArguments="viewModels:YourViewModel">
                   
    <!-- ... -->
    
</pages:ContentPage>
```

And in your code-behind:

```
using XamarinNavigation.Pages;

public class YourPage : ContentPage<YourViewModel>
{
    void SomeMethod()
    {
        ViewModel.YourViewModelMethod();
    }
}
```