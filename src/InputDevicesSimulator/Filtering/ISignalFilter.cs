using System.Collections.Generic;
using InputDevicesSimulator.Actions;

namespace InputDevicesSimulator.Filtering
{
    public interface ISignalFilter
    {
        IEnumerable<InputAction> Process(IEnumerable<InputAction> actions);
    }
}
