using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Filtering;
using InputDevicesSimulator.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InputDevicesSimulator.Tests.Recording
{
    [TestClass]
    public class CharactersConsolidationTests
    {
        [TestMethod]
        public void Keyboard_A_Key_Down_And_Up()
        {
            var cable = new CharactersConsolidation();
            var res = new List<InputAction>();

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.VK_A) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new KeyUp(VirtualKeyCode.VK_A) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new MouseMoveTo(0, 0) }) ?? new InputAction[0]);
            Assert.AreEqual(2, res.Count);

            Assert.IsInstanceOfType(res[0], typeof(InputCharacter));
            Assert.AreEqual(VirtualKeyCode.VK_A, (res[0] as InputCharacter).Character);

            Assert.IsInstanceOfType(res[1], typeof(MouseMoveTo));
            Assert.AreEqual(0, (res[1] as MouseMoveTo).X);
            Assert.AreEqual(0, (res[1] as MouseMoveTo).Y);
        }

        [TestMethod]
        public void Keyboard_Multiple_Keys_Down_And_Up()
        {
            var cable = new CharactersConsolidation();
            var res = new List<InputAction>();

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.VK_A) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.VK_B) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.VK_C) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.VK_D) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new MouseMoveTo(0, 0) }) ?? new InputAction[0]);
            Assert.AreEqual(3, res.Count);

            Assert.IsInstanceOfType(res[0], typeof(InputCharacters));
            Assert.AreEqual(VirtualKeyCode.VK_A, (res[0] as InputCharacters).Characters[0]);
            Assert.AreEqual(VirtualKeyCode.VK_B, (res[0] as InputCharacters).Characters[1]);
            Assert.AreEqual(VirtualKeyCode.VK_C, (res[0] as InputCharacters).Characters[2]);

            Assert.IsInstanceOfType(res[1], typeof(KeyDown));
            Assert.AreEqual(VirtualKeyCode.VK_D, (res[1] as KeyDown).KeyCode);

            Assert.IsInstanceOfType(res[2], typeof(MouseMoveTo));
            Assert.AreEqual(0, (res[2] as MouseMoveTo).X);
            Assert.AreEqual(0, (res[2] as MouseMoveTo).Y);
        }

        [TestMethod]
        public void Keyboard_Key_Down()
        {
            var cable = new CharactersConsolidation();
            var res = new List<InputAction>();

            res.AddRange(cable.Process(new[] { new KeyDown(VirtualKeyCode.VK_A) }) ?? new InputAction[0]);
            Assert.AreEqual(0, res.Count);

            res.AddRange(cable.Process(new[] { new MouseMoveTo(0, 0) }) ?? new InputAction[0]);
            Assert.AreEqual(2, res.Count);
            
            Assert.IsInstanceOfType(res[0], typeof(KeyDown));
            Assert.AreEqual(VirtualKeyCode.VK_A, (res[0] as KeyDown).KeyCode);

            Assert.IsInstanceOfType(res[1], typeof(MouseMoveTo));
            Assert.AreEqual(0, (res[1] as MouseMoveTo).X);
            Assert.AreEqual(0, (res[1] as MouseMoveTo).Y);
        }
    }
}
