using System;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Recording
{
    internal class KeyboardSimpleGlobalHook : BaseKeyboardGlobalHook
    {
        public event Action<VirtualKeyCode> KeyDown;
        public event Action<VirtualKeyCode> KeyUp;

        public KeyboardSimpleGlobalHook() : base()
        {
        }

        public override void Dispose()
        {
            this.KeyDown = null;
            this.KeyUp = null;

            base.Dispose();
        }

        protected override void KeyboardKeyUp(VirtualKeyCode key)
        {
            this.KeyUp?.Invoke(key);
        }

        protected override void KeyboardKeyDown(VirtualKeyCode key)
        {
            this.KeyDown?.Invoke(key);
        }
    }
}