﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinNavigation.Pages
{
    /// <summary>
    /// Represents a <see cref="ContentPage"/> with direct access to the ViewModel instance.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public class ContentPage<TViewModel> : ContentPage, IPage<TViewModel>
        where TViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the ViewModel instance.
        /// </summary>
        public TViewModel ViewModel
        {
            get => BindingContext as TViewModel;
            set => BindingContext = value;
        }

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        protected INavigationService NavigationService => ViewModel?.NavigationService;
    }
}
