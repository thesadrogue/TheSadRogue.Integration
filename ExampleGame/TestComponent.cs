using System;
using SadConsole;

namespace ExampleGame
{
    public class TestComponent : SadConsole.Components.UpdateComponent
    {
        private int _count;

        public override void Update(IScreenObject host, TimeSpan delta)
        {
            System.Diagnostics.Debug.WriteLine($"TestComponent Update: {_count++}");
        }
    }
}
