namespace NServiceLocator.Ninject
{
    using System;
    using System.Collections.Generic;
    using global::Ninject;
    using global::Ninject.Activation;
    using global::Ninject.Modules;
    using NServiceLocator;

    /// <summary>
    /// Example usage:
    /// <![CDATA[var serviceLocator = NinjectServiceLocator<MyNinjectModule>.GetInstance();]]>
    /// </summary>
    public class NinjectServiceLocator<TServiceLocatorModule> : IServiceLocator<IContext> where TServiceLocatorModule : NinjectModule, new()
    {
        private static readonly NinjectServiceLocator<TServiceLocatorModule> Instance = new NinjectServiceLocator<TServiceLocatorModule>();

        private NinjectServiceLocator() { Kernel = CreateKernel(); } //NOTE: BA; singleton.

        public IKernel Kernel { get; private set; }

        public static IServiceLocator<IContext> GetInstance(){ return Instance; }
        
        private static IKernel CreateKernel()
        {
            return new StandardKernel(new[] {new TServiceLocatorModule()});
        }

        public void BindToSelf(Type obj)
        {
            Kernel.Bind(obj).ToSelf();
        }

        public void BindToSelf<T>()
        {
            Kernel.Bind<T>().ToSelf();
        }

        public void BindToMethod<T>(Func<IContext,T> func)
        {
            Kernel.Bind<T>().ToMethod(func);
        }

        //public void BindToMethod(Type serviceType, Func<IContext, > func)
        //{
        //    Kernel.Bind(serviceType).ToMethod(func);
        //}

        public void BindToInterface<TImplementation, TInterface>(TImplementation type, TInterface tInterface) where TImplementation : TInterface where TInterface : Type
        {
            var r = Kernel.Bind(tInterface).To(type);
        }

        public void BindToInterface<TImplementation, TInterface>() where TImplementation : TInterface
        {
            Kernel.Bind<TInterface>().To<TImplementation>();
        }

        public void Release(object obj)
        {
            if(!Kernel.Release(obj))
            {
                throw new Exception("Failed to release object from service locator.");
            }
        }

        public object Locate(Type type)
        {
            return Kernel.Get(type);
        }

        public T Locate<T>() { return (T)(Kernel.Get(typeof(T))); }

        public IEnumerable<T> LocateAllImplementorsOf<T>()
        {
            return Kernel.GetAll<T>();
        }

        public IEnumerable<object> LocateAllImplementorsOf(Type type)
        {
            return Kernel.GetAll(type);
        }
    }
}