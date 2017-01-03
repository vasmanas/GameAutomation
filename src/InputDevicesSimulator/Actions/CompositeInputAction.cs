using System.Collections.Generic;

namespace InputDevicesSimulator.Actions
{
    public abstract class CompositeInputAction : InputAction
    {
        public abstract IEnumerable<InputAction> Translate();
    }
}
