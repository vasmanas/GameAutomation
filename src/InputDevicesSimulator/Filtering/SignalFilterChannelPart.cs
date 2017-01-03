using System;
using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;

namespace InputDevicesSimulator.Filtering
{
    public class SignalFilterChannelPart : ISignalChannelInput, ISignalChannelOutput
    {
        private readonly ISignalFilter filter;

        public event Action<IEnumerable<InputAction>> Observer;
        
        public SignalFilterChannelPart(ISignalFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            this.filter = filter;
        }

        public void Send(IEnumerable<InputAction> actions)
        {
            if (actions == null)
            {
                return;
            }

            var resultActions = this.filter.Process(actions);

            if (resultActions == null)
            {
                return;
            }

            this.Observer?.Invoke(resultActions);
        }
    }
}
