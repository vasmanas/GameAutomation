using System;
using System.Threading;
using InputDevicesSimulator.Simulation;

namespace GameAutomation
{
    public class Program
    {
        static void Main(string[] args)
        {
            var mouse = new MouseSimulator();
            mouse.MoveMouseTo(100, 100);
            Thread.Sleep(500);
            mouse.MoveMouseTo(100, 200);
            Thread.Sleep(500);
            mouse.MoveMouseTo(200, 200);
            mouse.LeftButtonDown();
            mouse.MoveMouseTo(300, 300);
            mouse.LeftButtonUp();

            //var macro = new Macro();
            //macro.Append(new MouseMoveTo(100, 100));
            //macro.Append(new WaitFor(500));
            //macro.Append(new MouseMoveTo(100, 200));
            //macro.Append(new WaitFor(500));
            //macro.Append(new MouseMoveTo(200, 200));
            //macro.Append(new MouseKeyDown(MouseButtons.Left));
            //macro.Append(new MouseMoveTo(300, 300));
            //macro.Append(new MouseKeyUp(MouseButtons.Left));

            //macro.Run();

            Console.ReadLine();
        }
    }
}
