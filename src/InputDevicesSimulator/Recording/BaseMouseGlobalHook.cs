using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Recording
{
    internal abstract class BaseMouseGlobalHook : IDisposable
    {
        private LowLevelProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        public BaseMouseGlobalHook()
        {
            this._proc = this.HookCallback;
            this._hookID = this.SetHook(this._proc);
        }

        public void Dispose()
        {
            WinApiMethods.UnhookWindowsHookEx(this._hookID);
        }

        protected abstract void Move(POINT pos);

        protected abstract void LeftButtonDown(POINT pos);

        protected abstract void LeftButtonUp(POINT pos);

        protected abstract void RightButtonDown(POINT pos);

        protected abstract void RightButtonUp(POINT pos);

        private IntPtr SetHook(LowLevelProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return WinApiMethods.SetWindowsHookEx((int)HookType.WH_MOUSE_LL, proc, WinApiMethods.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                if (MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
                {
                    this.Move(hookStruct.pt);
                }
                else if (MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
                {
                    this.LeftButtonDown(hookStruct.pt);
                }
                else if (MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
                {
                    this.LeftButtonUp(hookStruct.pt);
                }
                else if (MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam)
                {
                    this.RightButtonDown(hookStruct.pt);
                }
                else if (MouseMessages.WM_RBUTTONUP == (MouseMessages)wParam)
                {
                    this.RightButtonUp(hookStruct.pt);
                }

                // TODO: mouse wheel
            }

            return WinApiMethods.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
