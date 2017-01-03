using System.Collections.Generic;
using System.Linq;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Filtering
{
    // KeyDown(x)(N) -> KeyUp(x) | KeyDown(y)
    public class CharactersConsolidation :
        SignalFilter,
        ISignalFilterInput<KeyUp>,
        ISignalFilterInput<KeyDown>,
        ISignalFilterInput<MouseInputAction>
    {
        private readonly List<VirtualKeyCode> text = new List<VirtualKeyCode>();

        private VirtualKeyCode? lastKeyDown = null;

        public IEnumerable<InputAction> Process(KeyDown action)
        {
            if (this.lastKeyDown.HasValue)
            {
                this.text.Add(this.lastKeyDown.Value);
            }

            if (!this.CheckValidKeyCodes(action.KeyCode))
            {
                this.lastKeyDown = null;

                return new[] { action };
            }

            this.lastKeyDown = action.KeyCode;

            return null;
        }

        public IEnumerable<InputAction> Process(KeyUp action)
        {
            if (!this.CheckValidKeyCodes(action.KeyCode))
            {
                return new[] { action };
            }

            if (!this.lastKeyDown.HasValue)
            {
                return new[] { action };
            }

            this.text.Add(this.lastKeyDown.Value);

            if (this.lastKeyDown.Value == action.KeyCode)
            {
                this.lastKeyDown = null;
            }

            return null;
        }

        public IEnumerable<InputAction> Process(MouseInputAction action)
        {
            var result = new List<InputAction>();

            if (this.text.Any())
            {
                if (this.text.Count == 1)
                {
                    result.Add(new InputCharacter(this.text[0]));
                }
                else
                {
                    result.Add(new InputCharacters(this.text.ToArray()));
                }

                this.text.Clear();
            }

            if (this.lastKeyDown.HasValue)
            {
                result.Add(new KeyDown(this.lastKeyDown.Value));
            }

            result.Add(action);

            return result.ToArray();
        }

        private bool CheckValidKeyCodes(VirtualKeyCode keyCode)
        {
            return keyCode == VirtualKeyCode.SPACE
                || (VirtualKeyCode.VK_0 <= keyCode && keyCode <= VirtualKeyCode.VK_Z)
                || (VirtualKeyCode.NUMPAD0 <= keyCode && keyCode <= VirtualKeyCode.DIVIDE);
        }
    }
}
