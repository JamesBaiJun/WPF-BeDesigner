using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeDesigner.CustomerControls
{
    public interface IExecutable
    {
        bool IsExecuteState { get; set; }
        void Register();

        string ControlType { get; }
    }
}
