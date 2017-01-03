using System;
using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;
using InputDevicesSimulator.Recording;

namespace InputDevicesSimulator.Tests.Recording
{
    public class FakeSignaller : ISignalChannelOutput
    {
        public event Action<IEnumerable<InputAction>> Observer;

        public void Send(InputAction action)
        {
            this.Observer?.Invoke(new[] { action });
        }
    }
}
