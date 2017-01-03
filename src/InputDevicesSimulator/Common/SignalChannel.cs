using System;

namespace InputDevicesSimulator.Common
{
    public class SignalChannel
    {
        public static SignalChannel Create<TPart>(TPart part)
            where TPart : ISignalChannelOutput
        {
            if (part == null)
            {
                throw new ArgumentNullException("part");
            }
            
            return new SignalChannel(part);
        }

        private ISignalChannelOutput head;

        private ISignalChannelOutput tail;

        protected SignalChannel(ISignalChannelOutput head)
        {
            if (head == null)
            {
                throw new ArgumentNullException("head");
            }

            this.head = head;
            this.tail = head;
        }
        
        public SignalChannel Extend<TNextPart>(TNextPart part)
            where TNextPart : ISignalChannelInput
        {
            if (part == null)
            {
                throw new ArgumentNullException("part");
            }

            if (this.tail == null)
            {
                throw new InvalidOperationException("Channels tail is not output part");
            }

            this.tail.Observer += part.Send;

            var output = part as ISignalChannelOutput;
            if (output != null)
            {
                this.tail = output;
            }

            return this;
        }
    }
}
