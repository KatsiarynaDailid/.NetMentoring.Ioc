using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomIoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ExportAttribute : Attribute
    {
        public Type Type { get; private set; }

        public ExportAttribute()
        {

        }

        public ExportAttribute(Type type)
        {
            Type = type;
        }
    }
}
