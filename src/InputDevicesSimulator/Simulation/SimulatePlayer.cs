using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;

namespace InputDevicesSimulator.Simulation
{
    public class SimulatePlayer : ISignalChannelInput
    {
        private readonly MouseSimulator mouse;

        private readonly KeyboardSimulator keyboard;

        public SimulatePlayer()
        {
            this.mouse = new MouseSimulator();
            this.keyboard = new KeyboardSimulator();
        }

        public void Send(IEnumerable<InputAction> actions)
        {
            if (actions == null)
            {
                return;
            }

            foreach (dynamic action in actions)
            {
                this.Execute(action);
            }
        }

        private void Execute(MouseKeyUp action)
        {
            this.mouse.ButtonUp(action.Button);
        }

        private void Execute(MouseKeyDown action)
        {
            this.mouse.ButtonDown(action.Button);
        }

        private void Execute(MouseMoveTo action)
        {
            this.mouse.MoveMouseTo(action.X, action.Y);
        }

        private void Execute(KeyDown action)
        {
            this.keyboard.KeyDown(action.KeyCode);
        }

        private void Execute(KeyUp action)
        {
            this.keyboard.KeyUp(action.KeyCode);
        }

        private void Execute(WaitFor action)
        {
            Task.Delay(action.Miliseconds).Wait();
        }

        private void Execute(CompositeInputAction action)
        {
            this.Send(action.Translate());
        }

        private void Execute(InputAction action)
        {
            Debug.WriteLine(string.Format("Generic action: type: {0}", action.GetType().Name));
        }
    }
}
