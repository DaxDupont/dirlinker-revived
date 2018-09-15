using System;
using NUnit.Framework;
using JunctionPointer.Helpers.Interfaces;

namespace DirLinker.Tests.Helpers.OCInject
{
    [TestFixture]
    public class ClassFactoryTests
    {
        interface ITestClass { }
        class TestClass : ITestClass { }

        interface IDepend { }
        class Depend : IDepend { }


        interface ITestClassWithDepend { IDepend Depend {get; set;} }
        class TestClassWithDepend : ITestClassWithDepend
        {
            public IDepend Depend {get; set;}

            public TestClassWithDepend(IDepend idepend)
            {
                Depend = idepend;
            }
        }

        interface ITestClassWithDelegateFactory { ITestClassFactory Factory { get; set; } }
        delegate ITestClass ITestClassFactory();

        class TestClassWithDelegateFactory : ITestClassWithDelegateFactory
        {
            public ITestClassFactory Factory { get; set; }

            public TestClassWithDelegateFactory(ITestClassFactory delegateFactory)
            {
                Factory = delegateFactory;
            }
        }

        interface ITestClassTwoParam 
        {
            String One { get; }
            String Two { get; }
        }

        class TestClassTwoParam : ITestClassTwoParam 
        {
            public String One { get; set; }
            public String Two { get; set; }

            public TestClassTwoParam(String one, String two)
            {
                One = one;
                Two = two;
            }
        }

        delegate ITestClassTwoParam TestClassMultiParamFactory(String paramOne, String param2);

        interface ITestClassMultiParamFactoryConsumer { TestClassMultiParamFactory Factory { get; set; } }

        class TestClassMultiParamFactoryConsumer : ITestClassMultiParamFactoryConsumer
        {
            public TestClassMultiParamFactory Factory { get; set; }

            public TestClassMultiParamFactoryConsumer(TestClassMultiParamFactory delegateFactory)
            {
                Factory = delegateFactory;
            }
        }

        [Test]
        public void ManufactureType_ClassNoDependecies_CreatedSuccessfully()
        {

            IClassFactory testClssFactory = new JunctionPointer.Helpers.OCInject.ClassFactory();
            testClssFactory.RegisterType<ITestClass, TestClass>();

            ITestClass manufacturedType = testClssFactory.ManufactureType<ITestClass>();

            Assert.IsTrue(manufacturedType is TestClass);
        }

        [Test]
        public void ManufactureType_ClassWithDespendecies_CreatedSuccessfully()
        {
            IClassFactory testClassFactory = new JunctionPointer.Helpers.OCInject.ClassFactory();

            testClassFactory.RegisterType<IDepend, Depend>();
            testClassFactory.RegisterType<ITestClassWithDepend, TestClassWithDepend>();

            ITestClassWithDepend manufacturedType = testClassFactory.ManufactureType<ITestClassWithDepend>();

            Assert.IsTrue(manufacturedType is TestClassWithDepend);
            Assert.IsTrue(manufacturedType.Depend is Depend);
        }

        [Test]
        public void ManufactureType_Class_With_Despendecies_Dependency_Passed_In_Created_Successfully()
        {
            IClassFactory testClassFactory = new JunctionPointer.Helpers.OCInject.ClassFactory();

            testClassFactory.RegisterType<IDepend, Depend>();
            testClassFactory.RegisterType<ITestClassWithDepend, TestClassWithDepend>();

            IDepend newDepend = new Depend();
            ITestClassWithDepend manufacturedType = testClassFactory.ManufactureType<ITestClassWithDepend>(newDepend);

            Assert.AreSame(newDepend, manufacturedType.Depend);
        }

        [Test]
        public void ManufactureType_Type_accepts_delegate_factory_past_correctly()
        {
            IClassFactory testClassFactory = new JunctionPointer.Helpers.OCInject.ClassFactory();

            testClassFactory.RegisterType<ITestClass, TestClass>()
                .WithFactory<ITestClassFactory>();
            testClassFactory.RegisterType<ITestClassWithDelegateFactory, TestClassWithDelegateFactory>();

            ITestClassWithDelegateFactory manufacturedType = testClassFactory.ManufactureType<ITestClassWithDelegateFactory>();

            Assert.That(manufacturedType.Factory != null);
        }

        [Test]
        public void ManufactureType_Type_delegate_factory_manufactures_correct_type()
        {
            IClassFactory testClassFactory = new JunctionPointer.Helpers.OCInject.ClassFactory();

            testClassFactory.RegisterType<ITestClass, TestClass>()
                .WithFactory<ITestClassFactory>();
            testClassFactory.RegisterType<ITestClassWithDelegateFactory, TestClassWithDelegateFactory>();

            ITestClassWithDelegateFactory manufacturedType = testClassFactory.ManufactureType<ITestClassWithDelegateFactory>();
            ITestClass instance = manufacturedType.Factory();

            Assert.IsInstanceOf(typeof(TestClass), instance);

        }

        [Test]
        public void ManfactureType_Type_with_delegate_factory_that_accepts_two_params()
        {
            IClassFactory testClassFactory = new JunctionPointer.Helpers.OCInject.ClassFactory();

            testClassFactory.RegisterType<ITestClassTwoParam, TestClassTwoParam>()
                .WithFactory<TestClassMultiParamFactory>();
            testClassFactory.RegisterType<ITestClassMultiParamFactoryConsumer, TestClassMultiParamFactoryConsumer>();

            ITestClassMultiParamFactoryConsumer manufacturedType = testClassFactory.ManufactureType<ITestClassMultiParamFactoryConsumer>();

            Assert.That(manufacturedType.Factory != null);
        }

        [Test]
        public void ManfactureType_Type_with_delegate_factory_that_accepts_two_params_delegate_factory_produces_correct_class()
        {
            IClassFactory testClassFactory = new JunctionPointer.Helpers.OCInject.ClassFactory();

            testClassFactory.RegisterType<ITestClassTwoParam, TestClassTwoParam>()
                .WithFactory<TestClassMultiParamFactory>();

            testClassFactory.RegisterType<ITestClassMultiParamFactoryConsumer, TestClassMultiParamFactoryConsumer>();

            ITestClassMultiParamFactoryConsumer manufacturedType = testClassFactory.ManufactureType<ITestClassMultiParamFactoryConsumer>();

            ITestClassTwoParam typeCreatedFromFactory = manufacturedType.Factory("value One", "value Two");

            Assert.AreEqual("value One", typeCreatedFromFactory.One);
            Assert.AreEqual("value Two", typeCreatedFromFactory.Two);
        }
    }
}
