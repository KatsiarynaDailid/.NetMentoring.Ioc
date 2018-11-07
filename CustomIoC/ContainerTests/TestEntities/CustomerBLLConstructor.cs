using ContainerTests.TestEntities.Interfaces;
using CustomIoC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerTests.TestEntities
{
    [ImportConstructor]
    public class CustomerBLLConstructor
    {
        public CustomerBLLConstructor(ICustomerDAL dal, Logger logger) { }
    }
}
