using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinNavigation.Extensions;

namespace XamarinNavigation
{
    /// <summary>
    /// Provides properties and methods for handling page navigation in Xamarin.Forms applications.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private List<ViewToViewModelFactory> factories = new List<ViewToViewModelFactory>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class using defaults for view and ViewModel activators.
        /// </summary>
        public NavigationService()
            : this(Activator.Default, Activator.Default)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="defaultViewActivator">The <see cref="IActivator"/> instance to be used for view instanciation.</param>
        /// <param name="defaultViewModelActivator">The <see cref="IActivator"/> instance to be used for ViewModel instanciation.</param>
        public NavigationService(IActivator defaultViewActivator, IActivator defaultViewModelActivator)
            => (DefaultViewActivator, DefaultViewModelActivator) = (defaultViewActivator, defaultViewModelActivator);

        /// <summary>
        /// Gets or sets the <see cref="IActivator"/> instance which is used for view activation if no delegate for view creation is specified on the corresponding ViewModel registration.
        /// </summary>
        public IActivator DefaultViewActivator { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IActivator"/> instance which is used for ViewModel activation if no delegate for ViewModel creation is specified on the corresponding registration.
        /// </summary>
        public IActivator DefaultViewModelActivator { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="NavigationMode"/> which is used for executing the navigation process if no other <see cref="NavigationMode"/> was specified on the ViewModel registration.
        /// </summary>
        public NavigationMode DefaultNavigationType { get; set; } = NavigationMode.Default;

        #region Navigate

        /// <summary>
        /// Navigates to the page that was registered for the specified ViewModel type.
        /// </summary>
        /// <typeparam name="TViewModel">The ViewModel type.</typeparam>
        /// <param name="viewModel">The ViewModel instance.</param>
        /// <param name="options">An action which is called before the navigation progress is initiated. This action can be used to pass parameters to the ViewModel instance.</param>
        /// <param name="navigationMode">The <see cref="NavigationMode"/> which should be used for this single navigation process.</param>
        /// <returns></returns>
        public async Task Navigate<TViewModel>(TViewModel viewModel = null, Action<TViewModel> options = null, NavigationMode navigationMode = null) where TViewModel : ViewModelBase
        {
            ViewToViewModelFactory viewToViewModelFactory = GetViewToViewModelFactory<TViewModel, Page>();

            Page page = ResolveInternal<Page, TViewModel>(viewToViewModelFactory, viewModel, options);

            if (viewModel == null)
            {
                viewModel = page.BindingContext as TViewModel;
                if (viewModel != null)
                    options?.Invoke(viewModel);
            }

            if (navigationMode == null)
                navigationMode = viewToViewModelFactory.DefaultNavigationType ?? DefaultNavigationType;

            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                if (navigationMode == null)
                    await navigationPage.PushAsync(page);
                else
                    await navigationMode.Navigate(navigationPage.Navigation, page);
            }
            else
            {
                Application.Current.MainPage = page.ToNavigationPage();
            }

            await viewModel.OnNavigatedInternal();
        }

        #endregion

        #region RegisterViewModel

        /// <summary>
        /// Registers a ViewModel-to-view association.
        /// </summary>
        /// <typeparam name="TView">The view type.</typeparam>
        /// <typeparam name="TViewModel">The ViewModel type.</typeparam>
        /// <param name="createViewDelegate">The delegate that should be used for view activation. If this parameter is set, the <see cref="DefaultViewActivator"/> property is ignored.</param>
        /// <param name="createViewModelDelegate">The delegate that should be used for ViewModel activation. If this parameter is set, the <see cref="DefaultViewModelActivator"/> property is ignored.</param>
        /// <param name="defaultNavigationType">The <see cref="NavigationMode"/> that should be used when navigating to the specified ViewModel type.</param>
        /// <returns></returns>
        public INavigationService RegisterViewModel<TView, TViewModel>(Func<TView> createViewDelegate = null, Func<TViewModel> createViewModelDelegate = null, NavigationMode defaultNavigationType = null)
            where TView : VisualElement
            where TViewModel : ViewModelBase
        {
            factories.Add(
                new ViewToViewModelFactory(
                    viewType: typeof(TView),
                    viewModelType: typeof(TViewModel),
                    viewCreator: createViewDelegate ?? (() => DefaultViewActivator.GetInstance<TView>()),
                    viewModelCreator: createViewModelDelegate ?? (() => DefaultViewModelActivator.GetInstance<TViewModel>()),
                    defaultNavigationType: defaultNavigationType
                ));
            return this;
        }

        #endregion

        #region Resolve

        /// <summary>
        /// Gets the <see cref="Page"/> associated to the specified ViewModel type.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns></returns>
        public Page ResolvePage<TViewModel>()
            where TViewModel : ViewModelBase
        {
            return ResolveInternal<Page, TViewModel>();
        }

        public View ResolveView<TViewModel>()
            where TViewModel : ViewModelBase
        {
            return ResolveInternal<View, TViewModel>();
        }

        private TViewBaseType ResolveInternal<TViewBaseType, TViewModel>(ViewToViewModelFactory viewToViewModelFactory = null, TViewModel viewModel = null, Action<TViewModel> options = null)
            where TViewBaseType : VisualElement
            where TViewModel : ViewModelBase
        {
            ViewToViewModelFactory result = viewToViewModelFactory ?? GetViewToViewModelFactory<TViewModel, TViewBaseType>();

            if (result == null)
            {
                throw new InvalidOperationException($"The requested view model is not registered or its associated view is not derived from {typeof(TViewBaseType).FullName}.");
            }

            TViewBaseType view = result?.ViewCreator() as TViewBaseType;
            if (view == null)
            {
                return null;
            }
            else
            {
                if (viewModel == null)
                    viewModel = result.ViewModelCreator() as TViewModel;
                options?.Invoke(viewModel);
                viewModel.NavigationService = this;
                view.BindingContext = viewModel;
                return view;
            }
        }

        #endregion 

        /// <summary>
        /// Sets the main page of the app to the page associated to the specified ViewModel type.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns></returns>
        public INavigationService SetMainViewModel<TViewModel>()
            where TViewModel : ViewModelBase
        {
           ViewToViewModelFactory factory = factories.FirstOrDefault(f => f.ViewModelType == typeof(TViewModel));
            if (factory == null)
                throw new InvalidOperationException($"{typeof(TViewModel)} is not registered.");

            factories.ForEach(f => f.IsMainViewModel = false);

            Page page = ResolvePage<TViewModel>();
            Application.Current.MainPage = page.ToNavigationPage();
            
            factory.IsMainViewModel = true;

            page.Appearing += MainPage_Appearing;

            return this;
        }

        private async void MainPage_Appearing(object sender, EventArgs e)
        {
            if (((Page)sender).BindingContext is ViewModelBase viewModelBase)
                await viewModelBase.OnNavigatedInternal();
        }

        private ViewToViewModelFactory GetViewToViewModelFactory<TViewModel, TViewBaseType>()
            where TViewBaseType : VisualElement
            where TViewModel : ViewModelBase
            => factories.FirstOrDefault(
                factory => factory.ViewModelType == typeof(TViewModel) &&
                typeof(TViewBaseType).IsAssignableFrom(factory.ViewType));
    }
}
