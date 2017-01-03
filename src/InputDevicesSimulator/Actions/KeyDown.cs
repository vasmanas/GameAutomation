using System;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Actions
{
    public class KeyDown : KeyboardInputAction
    {
        public KeyDown(VirtualKeyCode keyCode)
        {
            this.KeyCode = keyCode;
        }

        public VirtualKeyCode KeyCode { get; private set; }
    }
}
