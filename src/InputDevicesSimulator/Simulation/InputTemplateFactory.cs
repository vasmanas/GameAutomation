using System;
using System.Collections.Generic;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Simulation
{
    internal static class InputFacade
    {
        public static INPUT MouseMoveRelative(int deltaX, int deltaY)
        {
            var movement = new INPUT { Type = (UInt32)InputType.Mouse };
            movement.Data.Mouse.Flags = (UInt32)MouseFlag.Move;
            movement.Data.Mouse.X = deltaX;
            movement.Data.Mouse.Y = deltaY;
            
            return movement;
        }

        public static INPUT MouseMoveAbsolute(double absoluteX, double absoluteY)
        {
            //// Get dpi settings
            //IntPtr hdc = NativeMethods.GetDC(IntPtr.Zero);
            //var a = NativeMethods.GetDeviceCaps(hdc, DeviceCap.HORZRES);
            //var b = NativeMethods.GetDeviceCaps(hdc, DeviceCap.VERTRES);

            var screenWidth = WinApiMethods.GetSystemMetrics(SystemMetric.SM_CXSCREEN) - 1;
            var screenHeight = WinApiMethods.GetSystemMetrics(SystemMetric.SM_CYSCREEN) - 1;

            var absX = Math.Round(Math.Abs(absoluteX * (65535.0f / screenWidth)), 0);
            var absY = Math.Round(Math.Abs(absoluteY * (65535.0f / screenHeight)), 0);

            var movement = new INPUT { Type = (UInt32)InputType.Mouse };
            movement.Data.Mouse.Flags = (UInt32)(MouseFlag.Move | MouseFlag.Absolute);
            movement.Data.Mouse.X = (int)absX;
            movement.Data.Mouse.Y = (int)absY;

            return movement;
        }

        public static INPUT MouseMoveAbsoluteOnVirtualDesktop(int x, int y)
        {
            var movement = new INPUT { Type = (UInt32)InputType.Mouse };
            movement.Data.Mouse.Flags = (UInt32)(MouseFlag.Move | MouseFlag.Absolute | MouseFlag.VirtualDesk);
            movement.Data.Mouse.X = x;
            movement.Data.Mouse.Y = y;

            return movement;
        }

        public static INPUT KeyDown(VirtualKeyCode keyCode)
        {
            var down =
                new INPUT
                {
                    Type = (UInt32)InputType.Keyboard,
                    Data =
                            {
                                Keyboard =
                                    new KEYBDINPUT
                                        {
                                            KeyCode = (UInt16)keyCode,
                                            Scan = 0,
                                            Flags = IsExtendedKey(keyCode) ? (UInt32) KeyboardFlag.ExtendedKey : 0,
                                            Time = 0,
                                            ExtraInfo = IntPtr.Zero
                                        }
                            }
                };

            return down;
        }

        public static INPUT KeyUp(VirtualKeyCode keyCode)
        {
            var up =
                new INPUT
                {
                    Type = (UInt32)InputType.Keyboard,
                    Data =
                            {
                                Keyboard =
                                    new KEYBDINPUT
                                        {
                                            KeyCode = (UInt16)keyCode,
                                            Scan = 0,
                                            Flags = (UInt32)(IsExtendedKey(keyCode)
                                                                  ? KeyboardFlag.KeyUp | KeyboardFlag.ExtendedKey
                                                                  : KeyboardFlag.KeyUp),
                                            Time = 0,
                                            ExtraInfo = IntPtr.Zero
                                        }
                            }
                };

            return up;
        }

        public static INPUT[] KeyPress(VirtualKeyCode keyCode)
        {
            return new[] { KeyDown(keyCode), KeyUp(keyCode) };
        }
        
        public static INPUT[] InputCharacter(char character)
        {
            UInt16 scanCode = character;

            var down = new INPUT
            {
                Type = (UInt32)InputType.Keyboard,
                Data =
                                       {
                                           Keyboard =
                                               new KEYBDINPUT
                                                   {
                                                       KeyCode = 0,
                                                       Scan = scanCode,
                                                       Flags = (UInt32)KeyboardFlag.Unicode,
                                                       Time = 0,
                                                       ExtraInfo = IntPtr.Zero
                                                   }
                                       }
            };

            var up = new INPUT
            {
                Type = (UInt32)InputType.Keyboard,
                Data =
                                     {
                                         Keyboard =
                                             new KEYBDINPUT
                                                 {
                                                     KeyCode = 0,
                                                     Scan = scanCode,
                                                     Flags =
                                                         (UInt32)(KeyboardFlag.KeyUp | KeyboardFlag.Unicode),
                                                     Time = 0,
                                                     ExtraInfo = IntPtr.Zero
                                                 }
                                     }
            };

            // Handle extended keys:
            // If the scan code is preceded by a prefix byte that has the value 0xE0 (224),
            // we need to include the KEYEVENTF_EXTENDEDKEY flag in the Flags property. 
            if ((scanCode & 0xFF00) == 0xE000)
            {
                down.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.ExtendedKey;
                up.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.ExtendedKey;
            }

            return new[] { down, up };
        }
        public static INPUT[] InputCharacters(string characters)
        {
            var inputs = new List<INPUT>();

            foreach (var character in characters)
            {
                inputs.AddRange(InputCharacter(character));
            }

            return inputs.ToArray();
        }

        public static INPUT MouseKeyDown(MouseButton button)
        {
            var buttonDown = new INPUT { Type = (UInt32)InputType.Mouse };
            buttonDown.Data.Mouse.Flags = (UInt32)ToMouseButtonDownFlag(button);
            if (button == MouseButton.X1)
            {
                buttonDown.Data.Mouse.MouseData = (UInt32)XButton.XButton1;
            }
            else if (button == MouseButton.X2)
            {
                buttonDown.Data.Mouse.MouseData = (UInt32)XButton.XButton2;
            }

            return buttonDown;
        }

        public static INPUT MouseKeyUp(MouseButton button)
        {
            var buttonUp = new INPUT { Type = (UInt32)InputType.Mouse };
            buttonUp.Data.Mouse.Flags = (UInt32)ToMouseButtonUpFlag(button);

            if (button == MouseButton.X1)
            {
                buttonUp.Data.Mouse.MouseData = (UInt32)XButton.XButton1;
            }
            else if (button == MouseButton.X2)
            {
                buttonUp.Data.Mouse.MouseData = (UInt32)XButton.XButton2;
            }

            return buttonUp;
        }

        public static INPUT[] MouseKeyClick(MouseButton button)
        {
            return new[] { MouseKeyDown(button), MouseKeyUp(button) };
        }

        public static INPUT[] MouseKeyDblClick(MouseButton button)
        {
            var inputs = new List<INPUT>();

            inputs.AddRange(MouseKeyClick(button));
            inputs.AddRange(MouseKeyClick(button));

            return inputs.ToArray();
        }

        public static INPUT MouseHorizontalWheelScroll(int scrollAmount)
        {
            var scroll = new INPUT { Type = (UInt32)InputType.Mouse };
            scroll.Data.Mouse.Flags = (UInt32)MouseFlag.HorizontalWheel;
            scroll.Data.Mouse.MouseData = (UInt32)scrollAmount;

            return scroll;
        }

        public static INPUT MouseVerticalWheelScroll(int scrollAmount)
        {
            var scroll = new INPUT { Type = (UInt32)InputType.Mouse };
            scroll.Data.Mouse.Flags = (UInt32)MouseFlag.VerticalWheel;
            scroll.Data.Mouse.MouseData = (UInt32)scrollAmount;

            return scroll;
        }

        private static MouseFlag ToMouseButtonDownFlag(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return MouseFlag.LeftDown;

                case MouseButton.Middle:
                    return MouseFlag.MiddleDown;

                case MouseButton.Right:
                    return MouseFlag.RightDown;

                case MouseButton.X1:
                case MouseButton.X2:
                    return MouseFlag.XDown;

                default:
                    throw new NotImplementedException();
            }
        }

        private static MouseFlag ToMouseButtonUpFlag(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return MouseFlag.LeftUp;

                case MouseButton.Middle:
                    return MouseFlag.MiddleUp;

                case MouseButton.Right:
                    return MouseFlag.RightUp;

                case MouseButton.X1:
                case MouseButton.X2:
                    return MouseFlag.XUp;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Determines if the <see cref="VirtualKeyCode"/> is an ExtendedKey
        /// </summary>
        /// <param name="keyCode">The key code.</param>
        /// <returns>true if the key code is an extended key; otherwise, false.</returns>
        /// <remarks>
        /// The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP, PAGE DOWN, and arrow keys in the clusters to the left of the numeric keypad; the NUM LOCK key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in the numeric keypad.
        /// 
        /// See http://msdn.microsoft.com/en-us/library/ms646267(v=vs.85).aspx Section "Extended-Key Flag"
        /// </remarks>
        private static bool IsExtendedKey(VirtualKeyCode keyCode)
        {
            if (keyCode == VirtualKeyCode.MENU ||
                keyCode == VirtualKeyCode.LMENU ||
                keyCode == VirtualKeyCode.RMENU ||
                keyCode == VirtualKeyCode.CONTROL ||
                keyCode == VirtualKeyCode.RCONTROL ||
                keyCode == VirtualKeyCode.INSERT ||
                keyCode == VirtualKeyCode.DELETE ||
                keyCode == VirtualKeyCode.HOME ||
                keyCode == VirtualKeyCode.END ||
                keyCode == VirtualKeyCode.PRIOR ||
                keyCode == VirtualKeyCode.NEXT ||
                keyCode == VirtualKeyCode.RIGHT ||
                keyCode == VirtualKeyCode.UP ||
                keyCode == VirtualKeyCode.LEFT ||
                keyCode == VirtualKeyCode.DOWN ||
                keyCode == VirtualKeyCode.NUMLOCK ||
                keyCode == VirtualKeyCode.CANCEL ||
                keyCode == VirtualKeyCode.SNAPSHOT ||
                keyCode == VirtualKeyCode.DIVIDE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}