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

        #region Navigate

        public async Task Navigate<TViewModel>(TViewModel viewModel = null) where TViewModel : ViewModelBase
        {
            Page page = ResolveInternal<Page, TViewModel>(viewModel);

            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.PushAsync(page);
            }
            else
            {
                Application.Current.MainPage = page is NavigationPage ? page : new NavigationPage(page);
            }
        }

        #endregion

        #region RegisterViewModel

        public INavigationService RegisterViewModel<TView, TViewModel>()
            where TView : VisualElement
            where TViewModel : ViewModelBase
        {
            factories.Add(new ViewToViewModelFactory(
                typeof(TView),
                typeof(TViewModel),
                () => DefaultViewActivator.GetInstance<TView>(),
                () => DefaultViewModelActivator.GetInstance<TViewModel>()));
            return this;
        }

        public INavigationService RegisterViewModel<TView, TViewModel>(Func<TView> createViewDelegate)
            where TView : VisualElement
            where TViewModel : ViewModelBase
        {
            factories.Add(new ViewToViewModelFactory(
                typeof(TView),
                typeof(TViewModel),
                createViewDelegate,
                () => DefaultViewModelActivator.GetInstance<TViewModel>()));
            return this;
        }

        public INavigationService RegisterViewModel<TView, TViewModel>(Func<TViewModel> createViewModelDelegate)
            where TView : VisualElement
            where TViewModel : ViewModelBase
        {
            factories.Add(new ViewToViewModelFactory(
                typeof(TView),
                typeof(TViewModel),
                () => DefaultViewModelActivator.GetInstance<TView>(),
                createViewModelDelegate));
            return this;
        }


        public INavigationService RegisterViewModel<TView, TViewModel>(Func<TView> createViewDelegate, Func<TViewModel> createViewModelDelegate)
            where TView : VisualElement
            where TViewModel : ViewModelBase
        {
            factories.Add(new ViewToViewModelFactory(
                typeof(TView),
                typeof(TViewModel),
                createViewDelegate,
                createViewModelDelegate));
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

        private TViewBaseType ResolveInternal<TViewBaseType, TViewModel>(TViewModel viewModel = null)
            where TViewBaseType : VisualElement
            where TViewModel : ViewModelBase
        {
            ViewToViewModelFactory result = factories.FirstOrDefault(
                factory => factory.ViewModelType == typeof(TViewModel) &&
                typeof(TViewBaseType).IsAssignableFrom(factory.ViewType));

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
            return this;
        }
    }
}
