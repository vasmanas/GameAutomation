using System;
using InputDevicesSimulator.Native;

namespace InputDevicesSimulator.Recording
{
    internal class MouseGlobalHook : BaseMouseGlobalHook
    {
        private POINT lastLeftMouseButtonDown;
        private POINT lastRightMouseButtonDown;

        public event Action<int, int> MouseMove;
        public event Action<int,int> LeftMouseButtonDown;
        public event Action<int, int> LeftMouseButtonUp;
        public event Action<int, int, int, int> LeftMouseButtonDrag;
        public event Action<int, int> RightMouseButtonDown;
        public event Action<int, int> RightMouseButtonUp;
        public event Action<int, int, int, int> RightMouseButtonDrag;

        public MouseGlobalHook() : base()
        {
        }

        public override void Dispose()
        {
            this.MouseMove = null;
            this.LeftMouseButtonDown = null;
            this.LeftMouseButtonUp = null;
            this.LeftMouseButtonDrag = null;
            this.RightMouseButtonDown = null;
            this.RightMouseButtonUp = null;
            this.RightMouseButtonDrag = null;

            base.Dispose();
        }

        protected override void Move(POINT pos)
        {
            this.MouseMove?.Invoke(pos.x, pos.y);
        }

        protected override void LeftButtonDown(POINT pos)
        {
            this.LeftMouseButtonDown?.Invoke(pos.x, pos.y);

            this.lastLeftMouseButtonDown = pos;
        }

        protected override void LeftButtonUp(POINT pos)
        {
            this.LeftMouseButtonDrag?.Invoke(
                this.lastLeftMouseButtonDown.x,
                this.lastLeftMouseButtonDown.y,
                pos.x,
                pos.y);

            this.LeftMouseButtonUp?.Invoke(pos.x, pos.y);
        }

        protected override void RightButtonDown(POINT pos)
        {
            this.RightMouseButtonDown?.Invoke(pos.x, pos.y);

            this.lastRightMouseButtonDown = pos;
        }

        protected override void RightButtonUp(POINT pos)
        {
            this.RightMouseButtonDrag?.Invoke(
                this.lastRightMouseButtonDown.x,
                this.lastRightMouseButtonDown.y,
                pos.x,
                pos.y);

            this.RightMouseButtonUp?.Invoke(pos.x, pos.y);
        }
    }
}
