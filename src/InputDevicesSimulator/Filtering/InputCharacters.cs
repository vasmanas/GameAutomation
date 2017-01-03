using System;
using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Filtering
{
    public class InputCharacters : CompositeInputAction
    {
        public InputCharacters(VirtualKeyCode[] characters)
        {
            this.Characters = characters;
        }

        public VirtualKeyCode[] Characters { get; private set; }

        public override IEnumerable<InputAction> Translate()
        {
            var result = new List<InputAction>();

            foreach (var character in this.Characters)
            {
                result.Add(new KeyDown(character));
                result.Add(new KeyUp(character));
            }

            return result;
        }
    }
}
