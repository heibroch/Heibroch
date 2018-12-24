using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heibroch.Common.Wpf
{
    public interface IInputHook
    {
        void Hook();
        void Unhook();
    }

    internal class HookBase : IInputHook
    {
        public void Hook()
        {
            throw new NotImplementedException();
        }

        public void Unhook()
        {
            throw new NotImplementedException();
        }
    }
}
