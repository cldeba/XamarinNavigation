using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinNavigation
{
    /// <summary>
    /// Represents the interface for pages that support direct access to their ViewModel instances.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public interface IPage<TViewModel>
    {
        TViewModel ViewModel { get; set; }
    }
}
