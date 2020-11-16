using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;
using Xunit;

namespace Tests.ComponentTests
{
    public class PlayerControlsTest
    {
        [Fact]
        public void NewPlayerControlsComponent()
        {
            var player = new RogueLikeEntity((0,0), 1, false, false, 1);
            var component = new PlayerControlsComponent();
            
            Assert.Equal(4, component.Motions.Count);
            Assert.Empty(component.Actions);
            
            player.AddComponent(component);
            
            Assert.Single(player.SadComponents);
            Assert.Single(player.GoRogueComponents);
        }
    }
}