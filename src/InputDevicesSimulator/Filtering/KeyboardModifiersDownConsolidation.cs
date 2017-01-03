using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Filtering
{
    // KeyDown(mod) + {any} + KeyDown(mod)
    public class KeyboardModifiersDownConsolidation : SignalFilter, ISignalFilterInput<KeyUp>, ISignalFilterInput<KeyDown>
    {
        private readonly List<VirtualKeyCode> modifiers = new List<VirtualKeyCode>();
        
        public IEnumerable<InputAction> Process(KeyDown action)
        {
            if (this.IsModifierKey(action.KeyCode))
            {
                if (!this.modifiers.Contains(action.KeyCode))
                {
                    this.modifiers.Add(action.KeyCode);

                    return new[] { action };
                }
            }
            else
            {
                return new[] { action };
            }

            return null;
        }

        public IEnumerable<InputAction> Process(KeyUp action)
        {
            if (this.IsModifierKey(action.KeyCode) && this.modifiers.Contains(action.KeyCode))
            {
                this.modifiers.Remove(action.KeyCode);
            }

            return new[] { action };
        }

        protected virtual bool IsModifierKey(VirtualKeyCode key)
        {
            return
                key == VirtualKeyCode.CAPITAL ||
                key == VirtualKeyCode.LSHIFT ||
                key == VirtualKeyCode.RSHIFT ||
                key == VirtualKeyCode.LCONTROL ||
                key == VirtualKeyCode.RCONTROL ||
                key == VirtualKeyCode.LMENU ||
                key == VirtualKeyCode.RMENU;
        }
    }
}
