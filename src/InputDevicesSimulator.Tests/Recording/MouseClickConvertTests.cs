using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Filtering;
using InputDevicesSimulator.Simulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InputDevicesSimulator.Tests.Recording
{
    [TestClass]
    public class MouseClickConvertTests
    {
        [TestMethod]
        public void Mouse_Click()
        {
            var cable = new MouseClickConvert();
            var res = new List<InputAction>();

            res.AddRange(cable.Process(new[] { new MouseKeyDown(MouseButton.Left) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new WaitFor(10) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new MouseKeyDown(MouseButton.Left) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new MouseKeyDown(MouseButton.Left) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new WaitFor(20) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new WaitFor(30) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new MouseKeyUp(MouseButton.Left) }) ?? new InputAction[0]);
            Assert.AreEqual(2, res.Count);

            Assert.IsInstanceOfType(res[0], typeof(WaitFor));
            Assert.AreEqual(60, (res[0] as WaitFor).Miliseconds);
            Assert.IsInstanceOfType(res[1], typeof(MouseClick));
            Assert.AreEqual(MouseButton.Left, (res[1] as MouseClick).Button);
        }

        [TestMethod]
        public void Mouse_Down_And_Hold()
        {
            var cable = new MouseClickConvert();
            var res = new List<InputAction>();

            res.AddRange(cable.Process(new[] { new MouseKeyDown(MouseButton.Left) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new WaitFor(100) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new WaitFor(5000) }) ?? new InputAction[0]);
            Assert.AreEqual(2, res.Count);

            Assert.IsInstanceOfType(res[0], typeof(MouseKeyDown));
            Assert.AreEqual(MouseButton.Left, (res[0] as MouseKeyDown).Button);
            Assert.IsInstanceOfType(res[1], typeof(WaitFor));
            Assert.AreEqual(5100, (res[1] as WaitFor).Miliseconds);

            res.AddRange(cable.Process(new[] { new MouseKeyUp(MouseButton.Left) }) ?? new InputAction[0]);
            Assert.AreEqual(3, res.Count);

            Assert.IsInstanceOfType(res[2], typeof(MouseKeyUp));
            Assert.AreEqual(MouseButton.Left, (res[2] as MouseKeyUp).Button);
        }

        [TestMethod]
        public void Mouse_Down_And_Mouse_Click()
        {
            var cable = new MouseClickConvert();
            var res = new List<InputAction>();

            res.AddRange(cable.Process(new[] { new WaitFor(100) }) ?? new InputAction[0]);
            Assert.AreEqual(1, res.Count);

            Assert.IsInstanceOfType(res[0], typeof(WaitFor));
            Assert.AreEqual(100, (res[0] as WaitFor).Miliseconds);

            res.AddRange(cable.Process(new[] { new MouseKeyDown(MouseButton.Left) }) ?? new InputAction[0]);
            Assert.AreEqual(1, res.Count);

            res.AddRange(cable.Process(new[] { new WaitFor(100) }) ?? new InputAction[0]);
            Assert.AreEqual(1, res.Count);

            res.AddRange(cable.Process(new[] { new MouseKeyDown(MouseButton.Right) }) ?? new InputAction[0]);
            Assert.AreEqual(3, res.Count);

            Assert.IsInstanceOfType(res[1], typeof(MouseKeyDown));
            Assert.AreEqual(MouseButton.Left, (res[1] as MouseKeyDown).Button);
            Assert.IsInstanceOfType(res[2], typeof(WaitFor));
            Assert.AreEqual(100, (res[2] as WaitFor).Miliseconds);

            res.AddRange(cable.Process(new[] { new WaitFor(200) }) ?? new InputAction[0]);
            Assert.AreEqual(3, res.Count);

            res.AddRange(cable.Process(new[] { new MouseKeyUp(MouseButton.Right) }) ?? new InputAction[0]);
            Assert.AreEqual(5, res.Count);

            Assert.IsInstanceOfType(res[3], typeof(WaitFor));
            Assert.AreEqual(200, (res[3] as WaitFor).Miliseconds);
            Assert.IsInstanceOfType(res[4], typeof(MouseClick));
            Assert.AreEqual(MouseButton.Right, (res[4] as MouseClick).Button);

            res.AddRange(cable.Process(new[] { new WaitFor(300) }) ?? new InputAction[0]);
            Assert.AreEqual(6, res.Count);

            Assert.IsInstanceOfType(res[5], typeof(WaitFor));
            Assert.AreEqual(300, (res[5] as WaitFor).Miliseconds);

            res.AddRange(cable.Process(new[] { new MouseKeyUp(MouseButton.Left) }) ?? new InputAction[0]);
            Assert.AreEqual(7, res.Count);

            Assert.IsInstanceOfType(res[6], typeof(MouseKeyUp));
            Assert.AreEqual(MouseButton.Left, (res[6] as MouseKeyUp).Button);
        }
    }
}
