using System;
using System.Collections.Generic;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;
using InputDevicesSimulator.Filtering;
using InputDevicesSimulator.Native;
using InputDevicesSimulator.Recording;

namespace InputDevicesSimulator
{
    public class InputControl
    {
        private readonly KeyboardGlobalHook control;

        private readonly SignalChannel recorder;

        private readonly SignalChannel player;

        public InputControl(ISignalChannelInput player = null)
        {            
            this.State = new IdleInputState();

            this.Tape = new Tape();

            this.InputListener = new InputListener();
            this.recorder =
                SignalChannel
                    .Create(this.InputListener)
                    //.Extend(new SignalFilterChannelPart(new StraightenMouseMovement()))
                    //.Extend(new SignalFilterChannelPart(new CharactersConsolidation()))                    
                    //.Extend(new SignalFilterChannelPart(new KeyboardModifiersDownConsolidation()))
                    //.Extend(new SignalFilterChannelPart(new MouseClickConvert()))
                    //.Extend(new SignalFilterChannelPart(new RemoveAction<WaitFor>()))
                    .Extend(new TapeWriter(this.Tape));

            this.TapeReader = new TapeReader(this.Tape);
            this.player = 
                SignalChannel
                    .Create(this.TapeReader)
                    .Extend(player ?? new DebugPlayer());
            
            this.control = new KeyboardGlobalHook();
            this.control.KeyUp += this.StartStopHotkeyDetect;
        }

        public InputState State { get; set; }

        public InputListener InputListener { get; private set; }

        public TapeReader TapeReader { get; private set; }

        public Tape Tape { get; private set; }

        public void Record()
        {
            this.State.Record(this);
        }

        public void Play()
        {
            this.State.Play(this);
        }

        public void Stop()
        {
            this.State.Stop(this);
        }

        public void Clear()
        {
            this.Tape.Clear();
        }

        /// <summary>
        /// Checks for Ctrl+Shift+S (Stop), Ctrl+Shift+R (Record) or Ctrl+Shift+P (Play)
        /// </summary>
        private void StartStopHotkeyDetect(VirtualKeyCode key, IEnumerable<VirtualKeyCode> modifiers)
        {
            if (key != VirtualKeyCode.VK_S && key != VirtualKeyCode.VK_R && key != VirtualKeyCode.VK_P)
            {
                return;
            }

            foreach (var modifier in modifiers)
            {
                if (modifier == VirtualKeyCode.LSHIFT || modifier == VirtualKeyCode.RSHIFT)
                {
                    continue;
                }

                if (modifier == VirtualKeyCode.LCONTROL || modifier == VirtualKeyCode.RCONTROL)
                {
                    continue;
                }

                return;
            }

            switch (key)
            {
                case VirtualKeyCode.VK_R:
                    {
                        this.Record();
                        break;
                    }

                case VirtualKeyCode.VK_P:
                    {
                        this.Play();
                        break;
                    }

                case VirtualKeyCode.VK_S:
                    {
                        this.Stop();
                        break;
                    }
            }
        }
    }
}
