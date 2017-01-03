using System;
using System.Collections.Generic;
using System.Linq;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;

namespace Visualizer.ViewModels
{
    public class ActionsEditViewModelWriter : ISignalChannelInput
    {
        private readonly ActionsEditViewModel model;

        public ActionsEditViewModelWriter(ActionsEditViewModel model)
        {
            this.model = model;
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
            this.model.AddItem<MouseKeyUp>("Mouse key up", action.Button.ToString());
        }

        private void Execute(MouseKeyDown action)
        {
            this.model.AddItem<MouseKeyDown>("Mouse key down", action.Button.ToString());
        }

        private void Execute(MouseMoveTo action)
        {
            this.model.AddItem<MouseMoveTo>("Mouse move to", string.Format("{0}, {1}", action.X, action.Y));
        }

        private void Execute(WaitFor action)
        {
            this.model.AddItem<WaitFor>("Wait for", string.Format("{0} ms", action.Miliseconds));
        }

        private void Execute(InputAction action)
        {
            this.model.AddItem<InputAction>(action.GetType().Name, "TODO: Define");
        }

        private void AddToFilter<T>(string name)
        {
            if (this.model.Filters.Any(e => e.Action.Equals(name)))
            {
                return;
            }

            this.model.Filters.Add(new FilterViewModel { Action = name, Type = typeof(T) });
        }
    }
}
