using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinNavigation
{
    /// <summary>
    /// Provides the default <see cref="IActivator"/> implementation.
    /// </summary>
    public static class Activator
    {
        static Activator()
        {
            Default = new DefaultActivator();
        }

        public static IActivator Default { get; }
    }
}
