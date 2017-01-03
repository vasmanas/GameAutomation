using System;
using System.Collections.Generic;
using InputDevicesSimulator.Actions;

namespace InputDevicesSimulator.Common
{
    public interface ISignalChannelOutput
    {
        event Action<IEnumerable<InputAction>> Observer;
    }
}
