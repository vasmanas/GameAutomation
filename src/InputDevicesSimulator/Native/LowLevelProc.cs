using System;

namespace InputDevicesSimulator.Native
{
    public delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);
}
