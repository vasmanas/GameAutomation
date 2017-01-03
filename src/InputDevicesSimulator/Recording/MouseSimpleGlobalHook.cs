using System;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Recording
{
    internal class MouseSimpleGlobalHook : BaseMouseGlobalHook
    {
        public event Action<int, int> MouseMove;
        public event Action<int,int> LeftMouseButtonDown;
        public event Action<int, int> LeftMouseButtonUp;
        public event Action<int, int> RightMouseButtonDown;
        public event Action<int, int> RightMouseButtonUp;

        public MouseSimpleGlobalHook() : base()
        {
        }

        protected override void Move(POINT pos)
        {
            this.MouseMove?.Invoke(pos.x, pos.y);
        }

        protected override void LeftButtonDown(POINT pos)
        {
            this.LeftMouseButtonDown?.Invoke(pos.x, pos.y);
        }

        protected override void LeftButtonUp(POINT pos)
        {
            this.LeftMouseButtonUp?.Invoke(pos.x, pos.y);
        }

        protected override void RightButtonDown(POINT pos)
        {
            this.RightMouseButtonDown?.Invoke(pos.x, pos.y);
        }

        protected override void RightButtonUp(POINT pos)
        {
            this.RightMouseButtonUp?.Invoke(pos.x, pos.y);
        }
    }
}
