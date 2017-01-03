using System.Collections.Generic;
using InputDevicesSimulator.Actions;

namespace InputDevicesSimulator.Common
{
    public interface ISignalChannelInput
    {
        void Send(IEnumerable<InputAction> actions);
    }
}
