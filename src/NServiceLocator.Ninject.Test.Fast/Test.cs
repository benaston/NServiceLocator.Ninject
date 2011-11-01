namespace NServiceLocator.Ninject.Test.Fast
{
    using System.Linq;
    using global::Ninject.Activation;
    using global::Ninject.Modules;
    using NUnit.Framework;

    public interface ITestIface {}

    public interface ITestIfaceGeneric<T> { }

    public class Test1 : ITestIface {}

    public class Test2 : ITestIface {}

    public class Test3 : ITestIfaceGeneric<string> { }

    public class Test4 : ITestIfaceGeneric<string> { }

    public class TestNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bindings.Clear();
            Bind<ITestIface>().To<Test1>();
            Bind<ITestIface>().To<Test2>();
            Bind<ITestIfaceGeneric<string>>().To<Test3>();
            Bind<ITestIfaceGeneric<string>>().To<Test4>();
        }
    }

    [TestFixture]
    internal class Tests
    {
        private IServiceLocator<IContext> _serviceLocator;

        [SetUp]
        public void Setup()
        {
            _serviceLocator = NinjectServiceLocator<TestNinjectModule>.GetInstance();
        }

        [Test]
        public void LocateAllImplementorsOf_NonGeneric_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _serviceLocator.LocateAllImplementorsOf<ITestIface>());

            Assert.That(_serviceLocator.LocateAllImplementorsOf<ITestIface>().Count() == 2);
        }

        [Test]
        public void LocateAllImplementorsOf()
        {
            Assert.DoesNotThrow(() => _serviceLocator.LocateAllImplementorsOf<ITestIface>());

            Assert.That(_serviceLocator.LocateAllImplementorsOf<ITestIfaceGeneric<string>>().Count() == 2);
        }
    }
}