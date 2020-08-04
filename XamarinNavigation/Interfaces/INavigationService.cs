using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace XamarinNavigation
{
    /// <summary>
    /// Base interface for Navigation service, providing methods for registering, resolving and navigation of ViewModels.
    /// </summary>
    public interface INavigationService
    {
        IActivator DefaultViewActivator { get; set; }
        IActivator DefaultViewModelActivator { get; set; }
        NavigationType DefaultNavigationType { get; set; }

        #region RegisterViewModel

        /// <summary>
        /// Registers a View-to-ViewModel relation with custom View resolving and custom ViewModel resolving.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="createViewDelegate"></param>
        /// <param name="createViewModelDelegate"></param>
        INavigationService RegisterViewModel<TView, TViewModel>(
            Func<TView> createViewDelegate = null,
            Func<TViewModel> createViewModelDelegate = null,
            NavigationType defaultNavigationType = null)
            where TView : VisualElement
            where TViewModel : ViewModelBase;

        #endregion

        /// <summary>
        /// Sets the main view model. The view model type has to be already registered beforehand.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        INavigationService SetMainViewModel<TViewModel>()
            where TViewModel : ViewModelBase;

        View ResolveView<TViewModel>() where TViewModel : ViewModelBase;
        Page ResolvePage<TViewModel>() where TViewModel : ViewModelBase;
        Task Navigate<TViewModel>(TViewModel viewModel = null, NavigationType navigationType = null) where TViewModel : ViewModelBase;
    }
}
