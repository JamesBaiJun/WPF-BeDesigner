using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeDesigner.Speical
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControlTypeAttribute : Attribute
    {
        public string Group { get; set; }
    }
}
