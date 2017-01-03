using System;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Actions
{
    public class KeyUp : KeyboardInputAction
    {
        public KeyUp(VirtualKeyCode keyCode)
        {
            this.KeyCode = keyCode;
        }

        public VirtualKeyCode KeyCode { get; private set; }
    }
}
