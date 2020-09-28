using System;

namespace XamarinNavigation
{
    /// <summary>
    /// Represents a <see cref="IActivator"/> implementation that uses the <see cref="Activator"/> to create instances.
    /// </summary>
    public class DefaultActivator : IActivator
    {
        public T GetInstance<T>()
        {
            return System.Activator.CreateInstance<T>();
        }
    }
}