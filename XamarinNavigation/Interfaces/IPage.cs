using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinNavigation
{
    public interface IPage<TViewModel>
    {
        TViewModel ViewModel { get; set; }
    }
}
