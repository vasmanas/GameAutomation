using System;
using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;
using InputDevicesSimulator.Filtering;
using InputDevicesSimulator.Native;
using InputDevicesSimulator.Recording;
using InputDevicesSimulator.Simulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InputDevicesSimulator.Tests.Recording
{
    [TestClass]
    public class BroadbandTests
    {
        [TestMethod]
        public void Keyboard_Modifier_Down_And_Up()
        {
            var signaller = new FakeSignaller();
            var tape = new Tape();
            var channel =
                SignalChannel
                    .Create(signaller)
                    .Extend(new SignalFilterChannelPart(new StraightenMouseMovement()))
                    .Extend(new SignalFilterChannelPart(new KeyboardModifiersDownConsolidation()))
                    .Extend(new SignalFilterChannelPart(new MouseClickConvert()))
                    .Extend(new TapeWriter(tape));

            signaller.Send(new MouseMoveTo(0, 0));
            Assert.AreEqual(0, tape.Count);

            signaller.Send(new KeyDown(VirtualKeyCode.LSHIFT));
            Assert.AreEqual(1, tape.Count);

            signaller.Send(new KeyDown(VirtualKeyCode.VK_A));
            Assert.AreEqual(2, tape.Count);

            signaller.Send(new MouseMoveTo(2, 2));
            Assert.AreEqual(2, tape.Count);

            signaller.Send(new KeyUp(VirtualKeyCode.VK_A));
            Assert.AreEqual(3, tape.Count);

            signaller.Send(new KeyDown(VirtualKeyCode.LSHIFT));
            Assert.AreEqual(3, tape.Count);

            signaller.Send(new KeyUp(VirtualKeyCode.LSHIFT));
            Assert.AreEqual(4, tape.Count);

            signaller.Send(new MouseMoveTo(3, 3));
            Assert.AreEqual(4, tape.Count);

            signaller.Send(new MouseKeyDown(MouseButton.Left));
            Assert.AreEqual(5, tape.Count);

            signaller.Send(new MouseKeyUp(MouseButton.Left));
            Assert.AreEqual(6, tape.Count);
        }
    }
}
