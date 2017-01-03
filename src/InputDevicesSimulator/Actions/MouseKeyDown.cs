using InputDevicesSimulator.Simulation;

namespace InputDevicesSimulator.Actions
{
    public class MouseKeyDown : MouseInputAction
    {
        public MouseKeyDown(MouseButton button)
        {
            this.Button = button;
        }

        public MouseButton Button { get; private set; }
    }
}
