using ContainerTests.TestEntities.Interfaces;
using CustomIoC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerTests.TestEntities
{
    [Export(typeof(ICustomerDAL))]
    public class CustomerDAL : ICustomerDAL
    { }
}
