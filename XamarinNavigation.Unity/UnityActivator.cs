using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace XamarinNavigation.Unity
{
    internal class UnityActivator : IActivator
    {
        internal UnityActivator(UnityContainer container)
        {
            Container = container;
        }

        private UnityContainer Container { get; }

        public T GetInstance<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
