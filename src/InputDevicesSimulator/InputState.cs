namespace InputDevicesSimulator
{
    public abstract class InputState
    {
        public abstract void Record(InputControl manager);

        public abstract void Play(InputControl manager);

        public abstract void Stop(InputControl manager);
    }
}
