using System.Runtime.InteropServices;

namespace InputDevicesSimulator.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int x;
        public int y;
    }
}
