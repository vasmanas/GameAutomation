using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;
using InputDevicesSimulator.Filtering;

namespace Visualizer.ViewModels
{
    public class ActionsEditViewModel
    {
        private readonly Tape tape;

        public ActionsEditViewModel(Tape tape)
        {
            this.Filters = new ObservableCollection<FilterViewModel>();
            this.Actions = new ObservableCollection<ActionViewModel>();

            this.tape = tape;
            this.FillActions();
        }

        public ObservableCollection<FilterViewModel> Filters { get; private set; }

        public ObservableCollection<ActionViewModel> Actions { get; private set; }

        public void Clear()
        {
            this.Actions.Clear();
            this.Filters.Clear();
        }

        public void AddItem<TAction>(string name, string value)
            where TAction : InputAction
        {
            var actionModel = new ActionViewModel { Action = name, Value = value };
            this.Actions.Add(actionModel);
            
            if (this.Filters.Any(e => e.Action.Equals(name)))
            {
                return;
            }

            var filter = new FilterViewModel { Action = name, Type = typeof(TAction) };
            this.Filters.Add(filter);
            filter.PropertyChanged += Filters_PropertyChanged;
        }

        private void Filters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.FillActions();
        }

        private void FillActions()
        {
            this.Actions.Clear();

            var reader = new TapeReader(this.tape);
            var readerChannel = SignalChannel.Create(reader);

            foreach (var filter in this.Filters)
            {
                if (!filter.Display)
                {
                    var ragt = typeof(RemoveAction<>);
                    var rat = ragt.MakeGenericType(filter.Type);

                    var part = (ISignalFilter)Activator.CreateInstance(rat);
                    readerChannel = readerChannel.Extend(new SignalFilterChannelPart(part));
                }
            }

            readerChannel = readerChannel.Extend(new ActionsEditViewModelWriter(this));

            reader.Start();
        }
    }
}
