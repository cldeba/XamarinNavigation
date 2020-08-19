using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
        public NavigationType DefaultNavigationType { get; set; } = NavigationType.Default;

        #region Navigate

        public async Task Navigate<TViewModel>(TViewModel viewModel = null, Action<TViewModel> options = null, NavigationType navigationType = null) where TViewModel : ViewModelBase
        {
            ViewToViewModelFactory viewToViewModelFactory = GetViewToViewModelFactory<TViewModel, Page>();

            Page page = ResolveInternal<Page, TViewModel>(viewToViewModelFactory, viewModel, options);

            if (viewModel == null)
            {
                viewModel = page.BindingContext as TViewModel;
                if (viewModel != null)
                    options?.Invoke(viewModel);
            }

            if (navigationType == null)
                navigationType = viewToViewModelFactory.DefaultNavigationType ?? DefaultNavigationType;

            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                if (navigationType == null)
                    await navigationPage.PushAsync(page);
                else
                    await navigationType.Navigate(navigationPage.Navigation, page);
            }
            else
            {
                Application.Current.MainPage = page is NavigationPage ? page : new NavigationPage(page);
            }

            await viewModel.OnNavigated();
        }

        #endregion

        #region RegisterViewModel

        public INavigationService RegisterViewModel<TView, TViewModel>(Func<TView> createViewDelegate = null, Func<TViewModel> createViewModelDelegate = null, NavigationType defaultNavigationType = null)
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
            factories.ForEach(f => f.IsMainViewModel = false);

            ViewToViewModelFactory factory = factories.FirstOrDefault(f => f.ViewModelType == typeof(TViewModel));
            if (factory == null)
                throw new InvalidOperationException($"{typeof(TViewModel)} is not registered.");

            Page page = ResolvePage<TViewModel>();
            Application.Current.MainPage = page is NavigationPage ? page : new NavigationPage(page);
            
            factory.IsMainViewModel = true;

            page.Appearing += MainPage_Appearing;
            // page.Appearing += (_, __) => page.Appearing -= MainPage_Appearing;

            return this;
        }

        private async void MainPage_Appearing(object sender, EventArgs e)
        {
            if (((Page)sender).BindingContext is ViewModelBase viewModelBase)
                await viewModelBase.OnNavigated();
        }

        private ViewToViewModelFactory GetViewToViewModelFactory<TViewModel, TViewBaseType>()
            where TViewBaseType : VisualElement
            where TViewModel : ViewModelBase
            => factories.FirstOrDefault(
                factory => factory.ViewModelType == typeof(TViewModel) &&
                typeof(TViewBaseType).IsAssignableFrom(factory.ViewType));
    }
}
