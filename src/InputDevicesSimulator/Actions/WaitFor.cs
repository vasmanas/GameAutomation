using System;

namespace InputDevicesSimulator.Actions
{
    public class WaitFor : InputAction
    {
        public WaitFor(int pauseMiliseconds)
        {
            if (pauseMiliseconds < 0)
            {
                throw new ArgumentException("Value must be non negative", "pauseMiliseconds");
            }

            this.Miliseconds = pauseMiliseconds;
        }

        public int Miliseconds { get; private set; }
    }
}
