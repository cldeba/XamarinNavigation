using System;

namespace XamarinNavigation
{
    public class DefaultActivator : IActivator
    {
        public T GetInstance<T>()
        {
            return System.Activator.CreateInstance<T>();
        }
    }
}