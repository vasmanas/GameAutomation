using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Simulation;

namespace InputDevicesSimulator.Filtering
{
    // MouseKeyDown + MouseKeyUp
    // MouseKeyDown(N) + WaitFor(N)(sum<=ClickInterval) + MouseKeyUp
    public class MouseClickConvert :
        SignalFilter,
        ISignalFilterInput<MouseKeyDown>,
        ISignalFilterInput<MouseKeyUp>,
        ISignalFilterInput<WaitFor>
    {
        public const int ClickInterval = 200;

        private MouseButton? button = null;

        private int intervalMs = 0;
        
        public virtual IEnumerable<InputAction> Process(MouseKeyUp action)
        {
            if (button.HasValue)
            {
                var results = new List<InputAction>();

                if (action.Button == button.Value)
                {
                    if (intervalMs > 0)
                    {
                        results.Add(new WaitFor(intervalMs));
                    }

                    results.Add(new MouseClick(button.Value, intervalMs));
                }
                else
                {
                    results.Add(new MouseKeyDown(button.Value));
                    results.Add(new WaitFor(intervalMs));
                    results.Add(action);
                }

                button = null;

                return results;
            }
            else
            {
                return new[] { action };
            }
        }

        public virtual IEnumerable<InputAction> Process(MouseKeyDown action)
        {
            if (button.HasValue)
            {
                if (button.Value != action.Button)
                {
                    var results = new List<InputAction>();

                    results.Add(new MouseKeyDown(button.Value));
                    results.Add(new WaitFor(intervalMs));

                    button = action.Button;
                    intervalMs = 0;

                    return results;
                }
            }
            else
            {
                button = action.Button;
                intervalMs = 0;
            }

            return null;
        }

        public virtual IEnumerable<InputAction> Process(WaitFor action)
        {
            if (button.HasValue)
            {
                intervalMs += action.Miliseconds;

                if (intervalMs > ClickInterval)
                {
                    var results = new List<InputAction>();

                    results.Add(new MouseKeyDown(button.Value));
                    results.Add(new WaitFor(intervalMs));

                    button = null;

                    return results;
                }
            }
            else
            {
                return new[] { action };
            }

            return null;
        }

        public override IEnumerable<InputAction> Process(InputAction action)
        {
            var results = new List<InputAction>();

            if (button.HasValue)
            {
                results.Add(new MouseKeyDown(button.Value));
                results.Add(new WaitFor(intervalMs));

                button = null;
            }

            results.Add(action);

            return results;
        }
    }
}
