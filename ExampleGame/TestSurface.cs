using SadConsole;
using SadConsole.Input;

namespace ExampleGame
{
    public class TestSurface : ScreenSurface
    {
        private int _count = 0;

        public TestSurface(int width, int height) : base(width, height)
        {

        }

        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            System.Diagnostics.Debug.WriteLine($"TestSurface ProcessMouse {_count++}!");

            //return base.ProcessMouse(state);
            base.ProcessMouse(state);

            return false;
        }
    }
}
