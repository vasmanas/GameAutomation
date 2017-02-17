using System;
using System.Collections.Generic;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Recording
{
    internal class KeyboardGlobalHook : BaseKeyboardGlobalHook
    {
        private readonly List<VirtualKeyCode> modifiers = new List<VirtualKeyCode>();

        public event Action<VirtualKeyCode, IEnumerable<VirtualKeyCode>> KeyDown;
        public event Action<VirtualKeyCode, IEnumerable<VirtualKeyCode>> KeyUp;
        public event Action<VirtualKeyCode> ModifierDown;
        public event Action<VirtualKeyCode> ModifierUp;

        public KeyboardGlobalHook() : base()
        {
        }

        public override void Dispose()
        {
            this.KeyDown = null;
            this.KeyUp = null;
            this.ModifierDown = null;
            this.ModifierUp = null;

            base.Dispose();
        }

        protected override void KeyboardKeyUp(VirtualKeyCode key)
        {
            if (this.IsModifierKey(key))
            {
                this.modifiers.Remove(key);

                this.ModifierUp?.Invoke(key);
            }
            else
            {
                this.KeyUp?.Invoke(key, this.modifiers.ToArray());
            }
        }

        protected override void KeyboardKeyDown(VirtualKeyCode key)
        {
            if (this.IsModifierKey(key))
            {
                if (!this.modifiers.Contains(key))
                {
                    this.ModifierDown?.Invoke(key);

                    this.modifiers.Add(key);
                }
            }
            else
            {
                this.KeyDown?.Invoke(key, this.modifiers.ToArray());
            }
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