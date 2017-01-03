using System;
using System.Collections.Generic;
using System.Linq;
using InputDevicesSimulator.Actions;

namespace InputDevicesSimulator.Filtering
{
    public abstract class SignalFilter : ISignalFilter, ISignalFilterInput<InputAction>
    {
        public virtual IEnumerable<InputAction> Process(IEnumerable<InputAction> actions)
        {
            if (actions == null)
            {
                return null;
            }

            var list = actions.ToList();

            if (list.Count == 0)
            {
                return null;
            }

            var results = new List<InputAction>();

            foreach (var action in actions)
            {
                results.AddRange(((dynamic)this).Process((dynamic)action) ?? new InputAction[0]);
            }

            if (results.Count == 0)
            {
                return null;
            }

            return results;
        }

        public virtual IEnumerable<InputAction> Process(InputAction action)
        {
            return new[] { action };
        }
    }
}
