using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Recording
{
    internal abstract class BaseKeyboardGlobalHook : IDisposable
    {
        private LowLevelProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        public BaseKeyboardGlobalHook()
        {
            this._proc = this.HookCallback;
            this._hookID = this.SetHook(this._proc);
        }
        
        public virtual void Dispose()
        {
            WinApiMethods.UnhookWindowsHookEx(this._hookID);
        }

        private IntPtr SetHook(LowLevelProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return WinApiMethods.SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL, proc, WinApiMethods.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var hookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                var vkc = (VirtualKeyCode)hookStruct.vkCode;

                //Debug.WriteLine("up   - {0}/{1}/{2}", KeyInterop.KeyFromVirtualKey((int)hookStruct.vkCode), hookStruct.vkCode, vkc);

                if (wParam == (IntPtr)KeyboardMessages.WM_KEYDOWN || wParam == (IntPtr)KeyboardMessages.WM_SYSKEYDOWN)
                {
                    this.KeyboardKeyDown(vkc);
                }
                else if (wParam == (IntPtr)KeyboardMessages.WM_KEYUP || wParam == (IntPtr)KeyboardMessages.WM_SYSKEYUP)
                {
                    this.KeyboardKeyUp(vkc);
                }
            }

            return WinApiMethods.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        protected abstract void KeyboardKeyUp(VirtualKeyCode key);

        protected abstract void KeyboardKeyDown(VirtualKeyCode key);
    }
}