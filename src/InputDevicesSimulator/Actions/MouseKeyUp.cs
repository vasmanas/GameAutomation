using InputDevicesSimulator.Simulation;

namespace InputDevicesSimulator.Actions
{
    public class MouseKeyUp : MouseInputAction
    {
        public MouseKeyUp(MouseButton button)
        {
            this.Button = button;
        }

        public MouseButton Button { get; private set; }
    }
}
