using System;
using System.Reflection;
using ContainerTests.TestEntities;
using ContainerTests.TestEntities.Interfaces;
using CustomIoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContainerTests
{
    [TestClass]
    public class ContainerTests
    {
        private Container container;

        [TestInitialize]
        public void ContainerTestsInitialize()
        {
            container = new Container();
        }

        [TestMethod]
        public void CreateInstance_UsingAssemblyAttributes_ConstructorDependencies()
        {
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var customerConstr = container.CreateInstance(typeof(CustomerBLLConstructor));
            var customerConstrGeneric = container.CreateInstance<CustomerBLLConstructor>();

            Assert.IsNotNull(customerConstr, "Customer instance was not created.");
            Assert.IsNotNull(customerConstrGeneric, "Customer instance was not created using generic method.");
            Assert.IsTrue(customerConstr.GetType() == typeof(CustomerBLLConstructor), "Wrong type returned by CreateInstance method.");
            Assert.IsTrue(customerConstrGeneric.GetType() == typeof(CustomerBLLConstructor), "Wrong type returned by CreateInstance generic method.");
        }

        [TestMethod]
        public void CreateInstance_UsingAssemblyAttributes_PropertyDependencies()
        {
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var customerProp = container.CreateInstance(typeof(CustomerBLLProperty));
            var customerPropGeneric = container.CreateInstance<CustomerBLLProperty>();

            Assert.IsNotNull(customerProp, "Customer instance was not created.");
            Assert.IsNotNull(customerPropGeneric, "Customer instance was not created using generic method.");
            Assert.IsTrue(customerProp.GetType() == typeof(CustomerBLLProperty), "Wrong type returned by CreateInstance method.");
            Assert.IsTrue(customerPropGeneric.GetType() == typeof(CustomerBLLProperty), "Wrong type returned by CreateInstance generic method.");
        }

        [TestMethod]
        public void CreateInstance_UsingAddType_PropertyDependencies()
        {
            container.AddType(typeof(CustomerBLLProperty));
            container.AddType(typeof(Logger));
            container.AddType(typeof(ICustomerDAL), typeof(CustomerDAL));

            var customerProp = (CustomerBLLProperty)container.CreateInstance(typeof(CustomerBLLProperty));
            var customerPropGeneric = container.CreateInstance<CustomerBLLProperty>();

            Assert.IsNotNull(customerProp.Logger, "Logger instance was not created.");
            Assert.IsNotNull(customerPropGeneric.Logger, "Logger instance was not created using generic method.");
            Assert.IsTrue(customerProp.Logger.GetType() == typeof(Logger), "Wrong type returned by CreateInstance method.");
            Assert.IsTrue(customerPropGeneric.Logger.GetType() == typeof(Logger), "Wrong type returned by CreateInstance generic method.");

            Assert.IsNotNull(customerProp.CustomerDAL, "CustomerDAL instance was not created.");
            Assert.IsNotNull(customerPropGeneric.CustomerDAL, "CustomerDAL instance was not created using generic method.");
            Assert.IsTrue(customerProp.CustomerDAL.GetType() == typeof(CustomerDAL), "Wrong type returned by CreateInstance method.");
            Assert.IsTrue(customerPropGeneric.CustomerDAL.GetType() == typeof(CustomerDAL), "Wrong type returned by CreateInstance generic method.");

            Assert.IsNotNull(customerProp, "Customer instance was not created.");
            Assert.IsNotNull(customerPropGeneric, "Customer instance was not created using generic method.");
            Assert.IsTrue(customerProp.GetType() == typeof(CustomerBLLProperty), "Wrong type returned by CreateInstance method.");
            Assert.IsTrue(customerPropGeneric.GetType() == typeof(CustomerBLLProperty), "Wrong type returned by CreateInstance generic method.");
        }
    }
}
