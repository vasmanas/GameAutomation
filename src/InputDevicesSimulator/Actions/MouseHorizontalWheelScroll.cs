namespace InputDevicesSimulator.Actions
{
    public class MouseHorizontalWheelScroll : MouseInputAction
    {
        public MouseHorizontalWheelScroll(int scrollAmount)
        {
            this.ScrollAmount = scrollAmount;
        }

        public int ScrollAmount { get; private set; }
    }
}
