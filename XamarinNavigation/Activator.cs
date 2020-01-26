using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinNavigation
{
    public static class Activator
    {
        static Activator()
        {
            Default = new DefaultActivator();
        }

        public static IActivator Default { get; }
    }
}
