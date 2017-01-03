using System;
using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Simulation;

namespace InputDevicesSimulator.Filtering
{
    public class MouseClick : CompositeInputAction
    {
        public MouseClick(MouseButton button, int heldPressedMs = 0)
        {
            if (heldPressedMs < 0)
            {
                throw new ArgumentException("Value must be positive or zero", "heldPressedMs");
            }

            this.Button = button;
            this.HeldPressedMs = heldPressedMs;
        }

        public MouseButton Button { get; private set; }

        public int HeldPressedMs { get; private set; }

        public override IEnumerable<InputAction> Translate()
        {
            if (this.HeldPressedMs > 0)
            {
                return
                    new InputAction[] { new MouseKeyDown(this.Button), new WaitFor(this.HeldPressedMs), new MouseKeyUp(this.Button) };
            }
            else
            {
                return
                    new InputAction[] { new MouseKeyDown(this.Button), new MouseKeyUp(this.Button) };
            }
        }
    }
}
