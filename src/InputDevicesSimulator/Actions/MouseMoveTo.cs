using System;

namespace InputDevicesSimulator.Actions
{
    public class MouseMoveTo : MouseInputAction
    {
        public MouseMoveTo(int xto, int yto)
        {
            /// HINT: Panasu, kad kazkiek i minusa gali nueiti.
            ////if (xto < 0)
            ////{
            ////    throw new ArgumentException("Value must be non negative", "xto");
            ////}

            ////if (yto < 0)
            ////{
            ////    throw new ArgumentException("Value must be non negative", "yto");
            ////}

            this.X = xto;
            this.Y = yto;
        }

        public int X { get; private set; }

        public int Y { get; private set; }
    }
}
