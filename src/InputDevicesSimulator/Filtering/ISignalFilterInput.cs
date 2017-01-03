using System.Collections.Generic;
using InputDevicesSimulator.Actions;

namespace InputDevicesSimulator.Filtering
{
    public interface ISignalFilterInput<TAction> where TAction : InputAction
    {
        IEnumerable<InputAction> Process(TAction action);
    }
}
