using System;
using System.Collections.Generic;
using System.Diagnostics;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;
using InputDevicesSimulator.Native;
using InputDevicesSimulator.Simulation;

namespace InputDevicesSimulator.Recording
{
    public class InputListener : ISignalChannelOutput
    {
        private readonly KeyboardSimpleGlobalHook keyboard;

        private readonly MouseSimpleGlobalHook mouse;

        private readonly Stopwatch inputTimer = new Stopwatch();

        private int lastMouseX = -1;

        private int lastMouseY = -1;

        private bool running = false;

        public event Action<IEnumerable<InputAction>> Observer;

        public InputListener()
        {
            this.mouse = new MouseSimpleGlobalHook();
            this.keyboard = new KeyboardSimpleGlobalHook();
        }

        public void Start()
        {
            if (this.running)
            {
                return;
            }

            this.running = true;
            
            this.inputTimer.Reset();

            this.lastMouseX = -1;
            this.lastMouseY = -1;

            // TODO: Customize with memento
            this.mouse.MouseMove += this.MouseMove;
            this.mouse.LeftMouseButtonDown += this.LeftMouseButtonDown;
            this.mouse.LeftMouseButtonUp += this.LeftMouseButtonUp;
            this.mouse.RightMouseButtonDown += this.RightMouseButtonDown;
            this.mouse.RightMouseButtonUp += this.RightMouseButtonUp;

            this.keyboard.KeyDown += this.KeyDown;
            this.keyboard.KeyUp += this.KeyUp;
        }

        public void Stop()
        {
            if (!this.running)
            {
                return;
            }

            this.running = false;

            this.inputTimer.Stop();

            // TODO: Customize with memento
            this.mouse.MouseMove -= this.MouseMove;
            this.mouse.LeftMouseButtonDown -= this.LeftMouseButtonDown;
            this.mouse.LeftMouseButtonUp -= this.LeftMouseButtonUp;
            this.mouse.RightMouseButtonDown -= this.RightMouseButtonDown;
            this.mouse.RightMouseButtonUp -= this.RightMouseButtonUp;

            this.keyboard.KeyDown -= this.KeyDown;
            this.keyboard.KeyUp -= this.KeyUp;
        }
        
        private void Send(InputAction action)
        {
            this.Observer?.Invoke(new[] { action });
        }

        private void InterruptSilence()
        {
            this.inputTimer.Stop();

            if (this.inputTimer.ElapsedMilliseconds > 0)
            {
                this.Send(new WaitFor((int)this.inputTimer.ElapsedMilliseconds));
            }

            this.inputTimer.Restart();
        }

        // TODO: Record wheel turn

        private void MouseMoveDetect(int x, int y)
        {
            if (this.lastMouseX == -1 || this.lastMouseX != x || this.lastMouseY != y)
            {
                this.lastMouseX = x;
                this.lastMouseY = y;

                this.Send(new MouseMoveTo(x, y));
            }
        }

        private void MouseMove(int x, int y)
        {
            this.InterruptSilence();
            this.MouseMoveDetect(x, y);
        }

        private void LeftMouseButtonDown(int x, int y)
        {
            this.InterruptSilence();
            this.MouseMoveDetect(x, y);
            this.Send(new MouseKeyDown(MouseButton.Left));
        }

        private void LeftMouseButtonUp(int x, int y)
        {
            this.InterruptSilence();
            this.MouseMoveDetect(x, y);
            this.Send(new MouseKeyUp(MouseButton.Left));
        }

        private void RightMouseButtonDown(int x, int y)
        {
            this.InterruptSilence();
            this.MouseMoveDetect(x, y);
            this.Send(new MouseKeyDown(MouseButton.Right));
        }

        private void RightMouseButtonUp(int x, int y)
        {
            this.InterruptSilence();
            this.MouseMoveDetect(x, y);
            this.Send(new MouseKeyUp(MouseButton.Right));
        }

        private void KeyDown(VirtualKeyCode key)
        {
            this.InterruptSilence();
            this.Send(new KeyDown(key));
        }

        private void KeyUp(VirtualKeyCode key)
        {
            this.InterruptSilence();
            this.Send(new KeyUp(key));
        }
    }
}
