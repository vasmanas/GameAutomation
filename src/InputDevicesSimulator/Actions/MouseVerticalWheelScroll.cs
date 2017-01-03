namespace InputDevicesSimulator.Actions
{
    public class MouseVerticalWheelScroll : MouseInputAction
    {
        public MouseVerticalWheelScroll(int scrollAmount)
        {
            this.ScrollAmount = scrollAmount;
        }

        public int ScrollAmount { get; private set; }
    }
}
