using System.Collections.Generic;
using InputDevicesSimulator.Actions;

namespace InputDevicesSimulator.Filtering
{
    // MouseMoveTo(N)
    // WaitFor(N) + MouseMoveTo(N) + WaitFor(N) + MouseMoveTo(N)
    public class StraightenMouseMovement :
        SignalFilter,
        ISignalFilterInput<MouseMoveBy>,
        ISignalFilterInput<MouseMoveTo>,
        ISignalFilterInput<WaitFor>
    {
        private readonly bool breaksOnlyOnMouseActions;

        private int waitForMs = 0;

        private int lastMouseX = -1;

        private int lastMouseY = -1;

        public StraightenMouseMovement(bool breaksOnlyOnMouseActions = true)
        {
            this.breaksOnlyOnMouseActions = breaksOnlyOnMouseActions;
        }
        
        public virtual IEnumerable<InputAction> Process(MouseMoveBy action)
        {
            lastMouseX += action.X;
            lastMouseY += action.Y;

            return null;
        }
        
        public virtual IEnumerable<InputAction> Process(MouseMoveTo action)
        {
            lastMouseX = action.X;
            lastMouseY = action.Y;

            return null;
        }

        public virtual IEnumerable<InputAction> Process(WaitFor action)
        {
            this.waitForMs += action.Miliseconds;

            return null;
        }

        public override IEnumerable<InputAction> Process(InputAction action)
        {
            if (this.breaksOnlyOnMouseActions && !(action is MouseInputAction))
            {
                return new[] { action };
            }

            var results = new List<InputAction>();

            if (this.waitForMs > 0)
            {
                results.Add(new WaitFor(this.waitForMs));

                this.waitForMs = 0;
            }

            if (lastMouseX >= 0)
            {
                results.Add(new MouseMoveTo(lastMouseX, lastMouseY));

                lastMouseX = -1;
                lastMouseY = -1;
            }

            results.Add(action);

            return results;
        }
    }
}
