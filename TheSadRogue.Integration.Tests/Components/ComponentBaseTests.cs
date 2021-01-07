using System;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;
using Xunit;

namespace TheSadRogue.Integration.Tests.Components
{
    public class ComponentBaseTests
    {
        [Fact]
        public void AddTest()
        {
            var component = new TestComponent();
            var entity = new RogueLikeEntity((0,0), Color.White,1);
            
            Assert.Empty(entity.SadComponents);
            Assert.Empty(entity.GoRogueComponents);
            
            entity.AddComponent(component);
            Assert.Single(entity.SadComponents);
            Assert.Single(entity.GoRogueComponents);
        }
    }

    public class TestComponent : RogueLikeComponentBase
    {
        public TestComponent() : base(true, true, true, true, 5)
        {
        }
    }
}