using InputDevicesSimulator;
using InputDevicesSimulator.Common;
using InputDevicesSimulator.Simulation;

namespace Visualizer.Global
{
    public static class Medium
    {
        static Medium()
        {
            //var player = new VisualPlayer();
            //var player = new DebugPlayer();
            var player = new SimulatePlayer();

            Medium.Control = new InputControl(player);
        }

        public static InputControl Control { get; private set; }
    }
}
