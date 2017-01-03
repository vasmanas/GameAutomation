namespace InputDevicesSimulator
{
    public class IdleInputState : InputState
    {
        public override void Play(InputControl manager)
        {
            manager.State = new PlayingInputState();
            manager.TapeReader.Start();
        }

        public override void Record(InputControl manager)
        {
            manager.State = new RecordingInputState();
            manager.InputListener.Start();
        }

        public override void Stop(InputControl manager)
        {
            // Nothig is beeng done, so no actions taken
        }
    }
}
