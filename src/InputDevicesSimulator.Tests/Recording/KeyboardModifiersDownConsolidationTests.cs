using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Filtering;
using InputDevicesSimulator.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InputDevicesSimulator.Tests.Recording
{
    [TestClass]
    public class KeyboardModifiersDownConsolidationTests
    {
        [TestMethod]
        public void Keyboard_Modifier_Down_And_Up()
        {
            var cable = new KeyboardModifiersDownConsolidation();
            var res = new List<InputAction>();

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.LSHIFT) }) ?? new InputAction[0]);
            Assert.AreEqual(1, res.Count);

            Assert.IsInstanceOfType(res[0], typeof(KeyDown));
            Assert.AreEqual(VirtualKeyCode.LSHIFT, (res[0] as KeyDown).KeyCode);

            res.AddRange(cable.Process(new[] { new WaitFor(10) }) ?? new InputAction[0]);
            Assert.AreEqual(2, res.Count);

            Assert.IsInstanceOfType(res[1], typeof(WaitFor));
            Assert.AreEqual(10, (res[1] as WaitFor).Miliseconds);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.LSHIFT) }) ?? new InputAction[0]);
            Assert.AreEqual(2, res.Count);

            res.AddRange(cable.Process(new[] { new WaitFor(20) }) ?? new InputAction[0]);
            Assert.AreEqual(3, res.Count);

            Assert.IsInstanceOfType(res[2], typeof(WaitFor));
            Assert.AreEqual(20, (res[2] as WaitFor).Miliseconds);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.LSHIFT) }) ?? new InputAction[0]);
            Assert.AreEqual(3, res.Count);

            res.AddRange(cable.Process(new[] { new WaitFor(30) }) ?? new InputAction[0]);
            Assert.AreEqual(4, res.Count);

            Assert.IsInstanceOfType(res[3], typeof(WaitFor));
            Assert.AreEqual(30, (res[3] as WaitFor).Miliseconds);

            res.AddRange(cable.Process(new[] { new KeyUp(VirtualKeyCode.LSHIFT) }) ?? new InputAction[0]);
            Assert.AreEqual(5, res.Count);

            Assert.IsInstanceOfType(res[4], typeof(KeyUp));
            Assert.AreEqual(VirtualKeyCode.LSHIFT, (res[4] as KeyUp).KeyCode);
        }

        [TestMethod]
        public void Keyboard_Nonmodifier_Down_And_Up()
        {
            var cable = new KeyboardModifiersDownConsolidation();
            var res = new List<InputAction>();

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.VK_A) }) ?? new InputAction[0]);
            Assert.AreEqual(1, res.Count);

            Assert.IsInstanceOfType(res[0], typeof(KeyDown));
            Assert.AreEqual(VirtualKeyCode.VK_A, (res[0] as KeyDown).KeyCode);

            res.AddRange(cable.Process(new[] { new WaitFor(10) }) ?? new InputAction[0]);
            Assert.AreEqual(2, res.Count);

            Assert.IsInstanceOfType(res[1], typeof(WaitFor));
            Assert.AreEqual(10, (res[1] as WaitFor).Miliseconds);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.VK_A) }) ?? new InputAction[0]);
            Assert.AreEqual(3, res.Count);

            Assert.IsInstanceOfType(res[2], typeof(KeyDown));
            Assert.AreEqual(VirtualKeyCode.VK_A, (res[2] as KeyDown).KeyCode);

            res.AddRange(cable.Process(new[] { new WaitFor(20) }) ?? new InputAction[0]);
            Assert.AreEqual(4, res.Count);

            Assert.IsInstanceOfType(res[3], typeof(WaitFor));
            Assert.AreEqual(20, (res[3] as WaitFor).Miliseconds);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.VK_A) }) ?? new InputAction[0]);
            Assert.AreEqual(5, res.Count);

            Assert.IsInstanceOfType(res[4], typeof(KeyDown));
            Assert.AreEqual(VirtualKeyCode.VK_A, (res[4] as KeyDown).KeyCode);

            res.AddRange(cable.Process(new[] { new WaitFor(30) }) ?? new InputAction[0]);
            Assert.AreEqual(6, res.Count);

            Assert.IsInstanceOfType(res[5], typeof(WaitFor));
            Assert.AreEqual(30, (res[5] as WaitFor).Miliseconds);

            res.AddRange(cable.Process(new[] { new KeyUp(VirtualKeyCode.VK_A) }) ?? new InputAction[0]);
            Assert.AreEqual(7, res.Count);

            Assert.IsInstanceOfType(res[6], typeof(KeyUp));
            Assert.AreEqual(VirtualKeyCode.VK_A, (res[6] as KeyUp).KeyCode);
        }

        [TestMethod]
        public void Multiple_Keyboard_Modifier_Down_And_Up()
        {
            var cable = new KeyboardModifiersDownConsolidation();
            var res = new List<InputAction>();

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.LSHIFT) }) ?? new InputAction[0]);
            Assert.AreEqual(1, res.Count);

            Assert.IsInstanceOfType(res[0], typeof(KeyDown));
            Assert.AreEqual(VirtualKeyCode.LSHIFT, (res[0] as KeyDown).KeyCode);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.LCONTROL) }) ?? new InputAction[0]);
            Assert.AreEqual(2, res.Count);

            Assert.IsInstanceOfType(res[1], typeof(KeyDown));
            Assert.AreEqual(VirtualKeyCode.LCONTROL, (res[1] as KeyDown).KeyCode);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.LMENU) }) ?? new InputAction[0]);
            Assert.AreEqual(3, res.Count);

            Assert.IsInstanceOfType(res[2], typeof(KeyDown));
            Assert.AreEqual(VirtualKeyCode.LMENU, (res[2] as KeyDown).KeyCode);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.LSHIFT) }) ?? new InputAction[0]);
            Assert.AreEqual(3, res.Count);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.LCONTROL) }) ?? new InputAction[0]);
            Assert.AreEqual(3, res.Count);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.LMENU) }) ?? new InputAction[0]);
            Assert.AreEqual(3, res.Count);

            res.AddRange(cable.Process(new[] { new KeyUp(VirtualKeyCode.LMENU) }) ?? new InputAction[0]);
            Assert.AreEqual(4, res.Count);

            Assert.IsInstanceOfType(res[3], typeof(KeyUp));
            Assert.AreEqual(VirtualKeyCode.LMENU, (res[3] as KeyUp).KeyCode);

            res.AddRange(cable.Process(new[] { new KeyUp(VirtualKeyCode.LSHIFT) }) ?? new InputAction[0]);
            Assert.AreEqual(5, res.Count);

            Assert.IsInstanceOfType(res[4], typeof(KeyUp));
            Assert.AreEqual(VirtualKeyCode.LSHIFT, (res[4] as KeyUp).KeyCode);

            res.AddRange(cable.Process(new[] { new KeyUp(VirtualKeyCode.LCONTROL) }) ?? new InputAction[0]);
            Assert.AreEqual(6, res.Count);

            Assert.IsInstanceOfType(res[5], typeof(KeyUp));
            Assert.AreEqual(VirtualKeyCode.LCONTROL, (res[5] as KeyUp).KeyCode);
        }
    }
}
