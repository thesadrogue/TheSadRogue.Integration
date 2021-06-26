using System;
using SadConsole;
using SadConsole.Input;

namespace ExampleGame
{
    public class TestScreen : ScreenObject
    {
        private int _count = 0;

        public override void Update(TimeSpan delta)
        {
            System.Diagnostics.Debug.WriteLine($"TestScreen Update {_count++}!");

            base.Update(delta);
        }
    }
}
