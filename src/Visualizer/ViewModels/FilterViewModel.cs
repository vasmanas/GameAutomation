using System;
using System.ComponentModel;

namespace Visualizer.ViewModels
{
    public class FilterViewModel : INotifyPropertyChanged
    {
        private bool display;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Display
        {
            get { return this.display; }
            set
            {
                if (this.display == value) return;
                this.display = value;
                this.RaisePropertyChanged("Display");
            }
        }

        public string Action { get; set; }

        public Type Type { get; set; }

        public FilterViewModel(bool display = true)
        {
            this.display = display;
        }

        private void RaisePropertyChanged(string propName)
        {
            var eh = PropertyChanged;
            if (eh != null)
            {
                eh(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
