// Copyright 2011, Ben Aston (ben@bj.ma).
// 
// This file is part of NServiceLocator.
// 
// NServiceLocator is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// NServiceLocator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with NServiceLocator.  If not, see <http://www.gnu.org/licenses/>.

namespace NServiceLocator.Ninject.Test.Fast
{
	using System.Linq;
	using NUnit.Framework;
	using global::Ninject.Activation;
	using global::Ninject.Modules;

	public interface ITestIface {}

	public interface ITestIfaceGeneric<T> {}

	public class Test1 : ITestIface {}

	public class Test2 : ITestIface {}

	public class Test3 : ITestIfaceGeneric<string> {}

	public class Test4 : ITestIfaceGeneric<string> {}

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
		#region Setup/Teardown

		[SetUp]
		public void Setup()
		{
			_serviceLocator = NinjectServiceLocator<TestNinjectModule>.GetInstance();
		}

		#endregion

		private IServiceLocator<IContext> _serviceLocator;

		[Test]
		public void LocateAllImplementorsOf()
		{
			Assert.DoesNotThrow(() => _serviceLocator.LocateAllImplementorsOf<ITestIface>());

			Assert.That(_serviceLocator.LocateAllImplementorsOf<ITestIfaceGeneric<string>>().Count() == 2);
		}

		[Test]
		public void LocateAllImplementorsOf_NonGeneric_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => _serviceLocator.LocateAllImplementorsOf<ITestIface>());

			Assert.That(_serviceLocator.LocateAllImplementorsOf<ITestIface>().Count() == 2);
		}
	}
}