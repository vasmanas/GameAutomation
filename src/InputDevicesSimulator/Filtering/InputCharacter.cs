using System;
using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Filtering
{
    public class InputCharacter : CompositeInputAction
    {
        public InputCharacter(VirtualKeyCode character)
        {
            this.Character = character;
        }

        public VirtualKeyCode Character { get; private set; }

        public override IEnumerable<InputAction> Translate()
        {
            return
                new InputAction[] { new KeyDown(this.Character), new KeyUp(this.Character) };
        }
    }
}
