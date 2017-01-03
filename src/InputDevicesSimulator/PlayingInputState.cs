namespace InputDevicesSimulator
{
    public class PlayingInputState : InputState
    {
        public override void Play(InputControl manager)
        {
            // During play no aditional actions are taken
        }

        public override void Record(InputControl manager)
        {
            manager.Stop();
            manager.Record();
        }

        public override void Stop(InputControl manager)
        {
            manager.State = new IdleInputState();
        }
    }
}
