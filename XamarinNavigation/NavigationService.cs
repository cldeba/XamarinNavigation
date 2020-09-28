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
    public class NavigationService : INavigationService
    {
        private List<ViewToViewModelFactory> factories = new List<ViewToViewModelFactory>();

        public NavigationService()
            : this(Activator.Default, Activator.Default)
        { }

        public NavigationService(IActivator defaultViewActivator, IActivator defaultViewModelActivator)
            => (DefaultViewActivator, DefaultViewModelActivator) = (defaultViewActivator, defaultViewModelActivator);

        public IActivator DefaultViewActivator { get; set; }
        public IActivator DefaultViewModelActivator { get; set; }
        public NavigationMode DefaultNavigationType { get; set; } = NavigationMode.Default;

        #region Navigate

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
