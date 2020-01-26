using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinNavigation
{
    /// <summary>
    /// Base interface for Navigation service, providing methods for registering, resolving and navigation of ViewModels.
    /// </summary>
    public interface INavigationService
    {
        IActivator DefaultViewActivator { get; set; }
        IActivator DefaultViewModelActivator { get; set; }

        #region RegisterViewModel

        /// <summary>
        /// Registers a View-to-ViewModel relation with default View resolving and default ViewModel resolving.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        INavigationService RegisterViewModel<TView, TViewModel>()
            where TView : VisualElement
            where TViewModel : ViewModelBase;

        /// <summary>
        /// Registers a View-to-ViewModel relation with custom View resolving and default ViewModel resolving.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="createViewDelegate"></param>
        INavigationService RegisterViewModel<TView, TViewModel>(
                Func<TView> createViewDelegate)
            where TView : VisualElement
            where TViewModel : ViewModelBase;

        /// <summary>
        /// Registers a View-to-ViewModel relation with default View resolving and custom ViewModel resolving.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="createViewModelDelegate"></param>
        INavigationService RegisterViewModel<TView, TViewModel>(
                Func<TViewModel> createViewModelDelegate)
            where TView : VisualElement
            where TViewModel : ViewModelBase;

        /// <summary>
        /// Registers a View-to-ViewModel relation with custom View resolving and custom ViewModel resolving.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="createViewDelegate"></param>
        /// <param name="createViewModelDelegate"></param>
        INavigationService RegisterViewModel<TView, TViewModel>(
            Func<TView> createViewDelegate,
            Func<TViewModel> createViewModelDelegate)
            where TView : VisualElement
            where TViewModel : ViewModelBase;

        #endregion

        View ResolveView<TViewModel>() where TViewModel : ViewModelBase;
        Page ResolvePage<TViewModel>() where TViewModel : ViewModelBase;
        Task Navigate<TViewModel>() where TViewModel : ViewModelBase;
    }
}
