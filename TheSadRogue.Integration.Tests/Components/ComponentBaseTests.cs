using System;
using SadConsole;
using SadConsole.Input;
using TheSadRogue.Integration.Components;
using Xunit;

namespace TheSadRogue.Integration.Tests.Components
{
    public class ComponentBaseTests
    {
        [Fact]
        public void AddTest()
        {
            var component = new TestComponent(false, false, false, false);
            var entity = new Integration.RogueLikeEntity((0, 0), 1);
            
            Assert.Empty(entity.SadComponents);
            Assert.Empty(entity.GoRogueComponents);
            
            entity.AddComponent(component);
            Assert.Single(entity.SadComponents);
            Assert.Single(entity.GoRogueComponents);
        }
    }

    public class TestComponent : RogueLikeComponentBase
    {
        public int RenderCount { get; private set; } = 0;
        public int UpdateCount { get; private set; } = 0;
        public int KeyboardCount { get; private set; } = 0;
        public int MouseCount { get; private set; } = 0;
        
        public TestComponent(bool isUpdate, bool isRender, bool isMouse, bool isKeyboard, int sortOrder = 5) : base(isUpdate, isRender, isMouse, isKeyboard, sortOrder)
        {
        }

        public override void Render(IScreenObject host, TimeSpan delta)
            => RenderCount++;


        public override void Update(IScreenObject host, TimeSpan delta)
            => UpdateCount++;

        public override void ProcessKeyboard(IScreenObject host, Keyboard keyboard, out bool handled)
            => handled = ++KeyboardCount > 1; 

        public override void ProcessMouse(IScreenObject host, MouseScreenObjectState state, out bool handled)
            => handled = ++MouseCount > 1;
    }
}