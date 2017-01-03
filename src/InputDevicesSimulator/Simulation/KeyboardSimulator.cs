using System;
using System.Collections.Generic;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Simulation
{
    public class KeyboardSimulator
    {
        /// <summary>
        /// Calls the Win32 SendInput method to simulate a KeyDown.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> to press</param>
        public void KeyDown(VirtualKeyCode keyCode)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.KeyDown(keyCode));
            handler.Handle();
        }

        /// <summary>
        /// Calls the Win32 SendInput method to simulate a KeyUp.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> to lift up</param>
        public void KeyUp(VirtualKeyCode keyCode)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.KeyUp(keyCode));
            handler.Handle();
        }

        /// <summary>
        /// Calls the Win32 SendInput method with a KeyDown and KeyUp message in the same input sequence in order to simulate a Key PRESS.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> to press</param>
        public void KeyPress(VirtualKeyCode keyCode)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.KeyPress(keyCode));
            handler.Handle();
        }

        /// <summary>
        /// Simulates a key press for each of the specified key codes in the order they are specified.
        /// </summary>
        /// <param name="keyCodes"></param>
        public void KeyPress(params VirtualKeyCode[] keyCodes)
        {
            if (keyCodes == null || keyCodes.Length == 0)
            {
                return;
            }

            var handler = new InputHandler();
            foreach (var keyCode in keyCodes)
            {
                handler.Append(InputFacade.KeyPress(keyCode));
            }

            handler.Handle();
        }

        /// <summary>
        /// Simulates a simple modified keystroke like CTRL-C where CTRL is the modifierKey and C is the key.
        /// The flow is Modifier KeyDown, Key Press, Modifier KeyUp.
        /// </summary>
        /// <param name="modifierKeyCode">The modifier key</param>
        /// <param name="keyCode">The key to simulate</param>
        public void ModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
        {
            ModifiedKeyStroke(new[] { modifierKeyCode }, new[] { keyCode });
        }

        /// <summary>
        /// Simulates a modified keystroke where there are multiple modifiers and one key like CTRL-ALT-C where CTRL and ALT are the modifierKeys and C is the key.
        /// The flow is Modifiers KeyDown in order, Key Press, Modifiers KeyUp in reverse order.
        /// </summary>
        /// <param name="modifierKeyCodes">The list of modifier keys</param>
        /// <param name="keyCode">The key to simulate</param>
        public void ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
        {
            ModifiedKeyStroke(modifierKeyCodes, new[] { keyCode });
        }

        /// <summary>
        /// Simulates a modified keystroke where there is one modifier and multiple keys like CTRL-K-C where CTRL is the modifierKey and K and C are the keys.
        /// The flow is Modifier KeyDown, Keys Press in order, Modifier KeyUp.
        /// </summary>
        /// <param name="modifierKey">The modifier key</param>
        /// <param name="keyCodes">The list of keys to simulate</param>
        public void ModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
        {
            ModifiedKeyStroke(new[] { modifierKey }, keyCodes);
        }

        /// <summary>
        /// Simulates a modified keystroke where there are multiple modifiers and multiple keys like CTRL-ALT-K-C where CTRL and ALT are the modifierKeys and K and C are the keys.
        /// The flow is Modifiers KeyDown in order, Keys Press in order, Modifiers KeyUp in reverse order.
        /// </summary>
        /// <param name="modifierKeyCodes">The list of modifier keys</param>
        /// <param name="keyCodes">The list of keys to simulate</param>
        public void ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes)
        {
            var handler = new InputHandler();

            ModifiersDown(handler, modifierKeyCodes);

            if (keyCodes != null)
            {
                foreach (var keyCode in keyCodes)
                {
                    handler.Append(InputFacade.KeyPress(keyCode));
                }
            }

            ModifiersUp(handler, modifierKeyCodes);

            handler.Handle();
        }

        /// <summary>
        /// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted text entry via the keyboard.
        /// </summary>
        /// <param name="text">The text to be simulated.</param>
        public void TextEntry(string text)
        {
            if (text.Length > UInt32.MaxValue / 2)
            {
                throw new ArgumentException(string.Format("The text parameter is too long. It must be less than {0} characters.", UInt32.MaxValue / 2), "text");
            }

            var handler = new InputHandler();
            handler.Append(InputFacade.InputCharacters(text));
            handler.Handle();
        }

        /// <summary>
        /// Simulates a single character text entry via the keyboard.
        /// </summary>
        /// <param name="character">The unicode character to be simulated.</param>
        public void TextEntry(char character)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.InputCharacter(character));
            handler.Handle();
        }

        private void ModifiersDown(InputHandler handler, IEnumerable<VirtualKeyCode> modifierKeyCodes)
        {
            if (modifierKeyCodes == null)
            {
                return;
            }

            foreach (var key in modifierKeyCodes)
            {
                handler.Append(InputFacade.KeyDown(key));
            }
        }

        private void ModifiersUp(InputHandler handler, IEnumerable<VirtualKeyCode> modifierKeyCodes)
        {
            if (modifierKeyCodes == null)
            {
                return;
            }

            // Key up in reverse (I miss LINQ)
            var stack = new Stack<VirtualKeyCode>(modifierKeyCodes);
            while (stack.Count > 0)
            {
                handler.Append(InputFacade.KeyUp(stack.Pop()));
            }
        }
    }
}
