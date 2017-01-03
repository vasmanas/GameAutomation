using System.Collections.Generic;
using System.Diagnostics;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;

namespace InputDevicesSimulator.Common
{
    public class DebugPlayer : ISignalChannelInput
    {
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
            Debug.WriteLine("MouseKeyUp: button - {0}", action.Button);
        }

        private void Execute(MouseKeyDown action)
        {
            Debug.WriteLine("MouseKeyDown: button - {0}", action.Button);
        }

        private void Execute(MouseMoveTo action)
        {
            Debug.WriteLine("MouseMoveTo: x - {0}, y - {1}", action.X, action.Y);
        }

        private void Execute(KeyDown action)
        {
            Debug.WriteLine("KeyDown: code - {0}", action.KeyCode);
        }

        private void Execute(KeyUp action)
        {
            Debug.WriteLine("KeyUp: code - {0}", action.KeyCode);
        }

        private void Execute(WaitFor action)
        {
            Debug.WriteLine("WaitFor: miliseconds - {0}", action.Miliseconds);
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
