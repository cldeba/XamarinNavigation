using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinNavigation
{
    public interface IActivator
    {
        T GetInstance<T>();
    }
}
