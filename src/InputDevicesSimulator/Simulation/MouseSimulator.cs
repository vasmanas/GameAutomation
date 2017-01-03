using System;

namespace InputDevicesSimulator.Simulation
{
    public class MouseSimulator
    {
        private const int MouseWheelClickSize = 120;

        /// <summary>
        /// Simulates mouse movement by the specified distance measured as a delta from the current mouse location in pixels.
        /// </summary>
        /// <param name="pixelDeltaX">The distance in pixels to move the mouse horizontally.</param>
        /// <param name="pixelDeltaY">The distance in pixels to move the mouse vertically.</param>
        public void MoveMouseBy(int pixelDeltaX, int pixelDeltaY)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.MouseMoveRelative(pixelDeltaX, pixelDeltaY));
            handler.Handle();
        }

        /// <summary>
        /// Simulates mouse movement to the specified location on the primary display device.
        /// </summary>
        /// <param name="absoluteX">The destination's absolute X-coordinate on the primary display device where 0 is the extreme left hand side of the display device and 65535 is the extreme right hand side of the display device.</param>
        /// <param name="absoluteY">The destination's absolute Y-coordinate on the primary display device where 0 is the top of the display device and 65535 is the bottom of the display device.</param>
        public void MoveMouseTo(double absoluteX, double absoluteY)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.MouseMoveAbsolute(absoluteX, absoluteY));
            handler.Handle();
        }

        /// <summary>
        /// Simulates mouse movement to the specified location on the Virtual Desktop which includes all active displays.
        /// </summary>
        /// <param name="absoluteX">The destination's absolute X-coordinate on the virtual desktop where 0 is the left hand side of the virtual desktop and 65535 is the extreme right hand side of the virtual desktop.</param>
        /// <param name="absoluteY">The destination's absolute Y-coordinate on the virtual desktop where 0 is the top of the virtual desktop and 65535 is the bottom of the virtual desktop.</param>
        public void MoveMouseToPositionOnVirtualDesktop(double absoluteX, double absoluteY)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.MouseMoveAbsoluteOnVirtualDesktop((int)Math.Truncate(absoluteX), (int)Math.Truncate(absoluteY)));
            handler.Handle();
        }

        /// <summary>
        /// Simulates a mouse button down gesture.
        /// </summary>
        /// <param name="button">The button.</param>
        public void ButtonDown(MouseButton button)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.MouseKeyDown(button));
            handler.Handle();
        }

        /// <summary>
        /// Simulates a mouse button up gesture.
        /// </summary>
        /// <param name="button">The button.</param>
        public void ButtonUp(MouseButton button)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.MouseKeyUp(button));
            handler.Handle();
        }

        /// <summary>
        /// Simulates a mouse click gesture.
        /// </summary>
        /// <param name="button">The button.</param>
        public void ButtonClick(MouseButton button)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.MouseKeyClick(button));
            handler.Handle();
        }

        /// <summary>
        /// Simulates a mouse button double-click gesture.
        /// </summary>
        /// <param name="button">The button.</param>
        public void ButtonDoubleClick(MouseButton button)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.MouseKeyDblClick(button));
            handler.Handle();
        }

        /// <summary>
        /// Simulates a mouse left button down gesture.
        /// </summary>
        public void LeftButtonDown()
        {
            this.ButtonDown(MouseButton.Left);
        }

        /// <summary>
        /// Simulates a mouse left button up gesture.
        /// </summary>
        public void LeftButtonUp()
        {
            this.ButtonUp(MouseButton.Left);
        }

        /// <summary>
        /// Simulates a mouse left-click gesture.
        /// </summary>
        public void LeftButtonClick()
        {
            this.ButtonClick(MouseButton.Left);
        }

        /// <summary>
        /// Simulates a mouse left button double-click gesture.
        /// </summary>
        public void LeftButtonDoubleClick()
        {
            this.ButtonDoubleClick(MouseButton.Left);
        }

        /// <summary>
        /// Simulates a mouse right button down gesture.
        /// </summary>
        public void RightButtonDown()
        {
            this.ButtonDown(MouseButton.Right);
        }

        /// <summary>
        /// Simulates a mouse right button up gesture.
        /// </summary>
        public void RightButtonUp()
        {
            this.ButtonUp(MouseButton.Right);
        }

        /// <summary>
        /// Simulates a mouse right button click gesture.
        /// </summary>
        public void RightButtonClick()
        {
            this.ButtonClick(MouseButton.Right);
        }

        /// <summary>
        /// Simulates a mouse right button double-click gesture.
        /// </summary>
        public void RightButtonDoubleClick()
        {
            this.ButtonDoubleClick(MouseButton.Right);
        }

        /// <summary>
        /// Simulates a mouse X button down gesture.
        /// </summary>
        /// <param name="buttonId">The button id, 1 or 2.</param>
        public void XButtonDown(int buttonId)
        {
            this.ButtonDown(buttonId == 1 ? MouseButton.X1 : MouseButton.X2);
        }

        /// <summary>
        /// Simulates a mouse X button up gesture.
        /// </summary>
        /// <param name="buttonId">The button id, 1 or 2.</param>
        public void XButtonUp(int buttonId)
        {
            this.ButtonUp(buttonId == 1 ? MouseButton.X1 : MouseButton.X2);
        }

        /// <summary>
        /// Simulates a mouse X button click gesture.
        /// </summary>
        /// <param name="buttonId">The button id, 1 or 2.</param>
        public void XButtonClick(int buttonId)
        {
            this.ButtonClick(buttonId == 1 ? MouseButton.X1 : MouseButton.X2);
        }

        /// <summary>
        /// Simulates a mouse X button double-click gesture.
        /// </summary>
        /// <param name="buttonId">The button id, 1 or 2.</param>
        public void XButtonDoubleClick(int buttonId)
        {
            this.ButtonDoubleClick(buttonId == 1 ? MouseButton.X1 : MouseButton.X2);
        }

        /// <summary>
        /// Simulates mouse vertical wheel scroll gesture.
        /// </summary>
        /// <param name="scrollAmountInClicks">The amount to scroll in clicks. A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user.</param>
        public void VerticalScroll(int scrollAmountInClicks)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.MouseVerticalWheelScroll(scrollAmountInClicks * MouseWheelClickSize));
            handler.Handle();
        }

        /// <summary>
        /// Simulates a mouse horizontal wheel scroll gesture. Supported by Windows Vista and later.
        /// </summary>
        /// <param name="scrollAmountInClicks">The amount to scroll in clicks. A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left.</param>
        public void HorizontalScroll(int scrollAmountInClicks)
        {
            var handler = new InputHandler();
            handler.Append(InputFacade.MouseHorizontalWheelScroll(scrollAmountInClicks * MouseWheelClickSize));
            handler.Handle();
        }
    }
}