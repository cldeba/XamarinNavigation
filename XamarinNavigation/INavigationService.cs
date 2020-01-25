using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinNavigation
{
    /// <summary>
    /// Base interface for Navigation service, providing methods for registering, resolving and navigation of ViewModels.
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
        void RegisterViewModel<TView, TViewModel>()
            where TView : VisualElement
            where TViewModel : ViewModelBase;

        /// <summary>
        /// Registers a View-to-ViewModel relation with custom View resolving and default ViewModel resolving.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="createViewDelegate"></param>
        void RegisterViewModel<TView, TViewModel>(
                Func<TView> createViewDelegate)
            where TView : VisualElement
            where TViewModel : ViewModelBase;

        /// <summary>
        /// Registers a View-to-ViewModel relation with default View resolving and custom ViewModel resolving.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="createViewModelDelegate"></param>
        void RegisterViewModel<TView, TViewModel>(
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
        void RegisterViewModel<TView, TViewModel>(
            Func<TView> createViewDelegate,
            Func<TViewModel> createViewModelDelegate)
            where TView : VisualElement
            where TViewModel : ViewModelBase;

        #endregion

        View ResolveView<TViewModel>();
        Page ResolvePage<TViewModel>();
        Task Navigate<TViewModel>();
    }
}
