using System;
using System.Collections.Generic;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Recording
{
    internal class KeyboardNonRepeatGlobalHook : BaseKeyboardGlobalHook
    {
        private readonly List<VirtualKeyCode> modifiers = new List<VirtualKeyCode>();

        public event Action<VirtualKeyCode> KeyDown;
        public event Action<VirtualKeyCode> KeyUp;

        public KeyboardNonRepeatGlobalHook() : base()
        {
        }

        protected override void KeyboardKeyUp(VirtualKeyCode key)
        {
            if (this.IsModifierKey(key))
            {
                this.modifiers.Remove(key);
            }

            this.KeyUp?.Invoke(key);
        }

        protected override void KeyboardKeyDown(VirtualKeyCode key)
        {
            if (this.IsModifierKey(key))
            {
                if (!this.modifiers.Contains(key))
                {
                    this.modifiers.Add(key);

                    this.KeyDown?.Invoke(key);
                }

                return;
            }

            this.KeyDown?.Invoke(key);
        }

        private bool IsModifierKey(VirtualKeyCode key)
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