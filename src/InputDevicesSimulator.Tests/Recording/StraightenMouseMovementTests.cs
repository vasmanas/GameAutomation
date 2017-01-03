using System;
using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Filtering;
using InputDevicesSimulator.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InputDevicesSimulator.Tests.Recording
{
    [TestClass]
    public class StraightenMouseMovementTests
    {
        [TestMethod]
        public void Mouse_Move_And_Pause_Aggregation()
        {
            var cable = new StraightenMouseMovement(false);
            var res = new List<InputAction>();

            res.AddRange(cable.Process(new[] { new WaitFor(10) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new MouseMoveTo(10, 10) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new WaitFor(20) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new MouseMoveTo(20, 20) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.LSHIFT) }) ?? new InputAction[0]);
            Assert.AreEqual(3, res.Count);
            Assert.IsInstanceOfType(res[0], typeof(WaitFor));
            Assert.AreEqual(30, (res[0] as WaitFor).Miliseconds);
            Assert.IsInstanceOfType(res[1], typeof(MouseMoveTo));
            Assert.AreEqual(20, (res[1] as MouseMoveTo).X);
            Assert.AreEqual(20, (res[1] as MouseMoveTo).Y);
        }

        [TestMethod]
        public void Mouse_Move_To_Single()
        {
            var cable = new StraightenMouseMovement(false);
            var res = new List<InputAction>();

            res.AddRange(cable.Process(new[] { new MouseMoveTo(10, 10) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new MouseMoveTo(20, 20) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new MouseMoveTo(30, 30) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new MouseMoveTo(40, 40) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.LSHIFT) }) ?? new InputAction[0]);
            Assert.AreEqual(2, res.Count);
            Assert.IsInstanceOfType(res[0], typeof(MouseMoveTo));
            Assert.AreEqual(40, (res[0] as MouseMoveTo).X);
            Assert.AreEqual(40, (res[0] as MouseMoveTo).Y);
        }
    }
}
