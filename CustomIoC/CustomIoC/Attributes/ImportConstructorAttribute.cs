using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomIoC.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
    public sealed class ImportConstructorAttribute : Attribute
    {

    }
}
