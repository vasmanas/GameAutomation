using System;
using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;

namespace InputDevicesSimulator.Common
{
    public class TapeWriter : ISignalChannelInput
    {
        private Tape tape;

        public TapeWriter(Tape tape = null)
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

        public void Send(IEnumerable<InputAction> actions)
        {
            this.tape.AddRange(actions);
        }

        public void Clear()
        {
            this.tape.Clear();
        }
    }
}
