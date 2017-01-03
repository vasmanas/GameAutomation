using System.Collections.Generic;
using InputDevicesSimulator.Actions;

namespace InputDevicesSimulator.Filtering
{
    public class RemoveAction<TAction> : SignalFilter, ISignalFilterInput<TAction>
        where TAction : InputAction
    {
        public IEnumerable<InputAction> Process(TAction action)
        {
            return null;
        }
    }
}
