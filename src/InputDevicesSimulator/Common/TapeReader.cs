using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;
using InputDevicesSimulator.Recording;

namespace InputDevicesSimulator.Common
{
    public class TapeReader : ISignalChannelOutput
    {
        public event Action<IEnumerable<InputAction>> Observer;

        private Tape tape;

        public TapeReader(Tape tape = null)
        {
            this.tape = tape ?? new Tape();
        }

        public Tape Tape
        {
            get
            {
                return this.tape;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.tape = value;
            }
        }

        public void Start()
        {
            this.Observer?.Invoke(this.tape);
        }
    }
}
