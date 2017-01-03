namespace InputDevicesSimulator
{
    public class RecordingInputState : InputState
    {
        public override void Play(InputControl manager)
        {
            manager.Stop();
            manager.Play();
        }

        public override void Record(InputControl manager)
        {
            // During recordind no aditiona actions are taken
        }

        public override void Stop(InputControl manager)
        {
            manager.InputListener.Stop();
            manager.State = new IdleInputState();
        }
    }
}
