using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Simulation
{
    internal class InputHandler
    {
        private readonly List<INPUT> inputs = new List<INPUT>();
        
        public void Handle()
        {
            if (this.inputs.Count == 0)
            {
                return;
            }

            var inArr = this.inputs.ToArray();

            var successful = WinApiMethods.SendInput((UInt32)inArr.Length, inArr, Marshal.SizeOf(typeof(INPUT)));

            this.inputs.Clear();

            if (successful != inArr.Length)
            {
                throw new Exception("Some simulated input commands were not sent successfully. The most common reason for this happening are the security features of Windows including User Interface Privacy Isolation (UIPI). Your application can only send commands to applications of the same or lower elevation. Similarly certain commands are restricted to Accessibility/UIAutomation applications. Refer to the project home page and the code samples for more information.");
            }
        }

        public void Append(INPUT input)
        {
            this.inputs.Add(input);
        }

        public void Append(INPUT[] inputs)
        {
            this.inputs.AddRange(inputs);
        }
    }
}