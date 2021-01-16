using System;
using SadConsole;

namespace TheSadRogue.Integration.Tests
{
    /// <summary>
    /// A test fixture for initializing SadConsole to run unit tests.
    /// </summary>
    public class SadConsoleFixture : IDisposable
    {
        public GameHost MockHost { get; }

        public SadConsoleFixture()
        {
            MockHost = new Mocks.MockGameHost();
        }


        public void Dispose() => MockHost?.Dispose();
    }
}
