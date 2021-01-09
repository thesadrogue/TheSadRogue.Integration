using SadConsole;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;
using Xunit;

namespace TheSadRogue.Integration.Tests.Components
{
    public class PlayerControlsTest
    {
        [Fact]
        public void NewPlayerControlsComponent()
        {
            var player = new TheSadRogue.Integration.RogueLikeEntity((0,0), Color.White,1);
            var component = new PlayerControlsComponent();
            
            Assert.Equal(4, component.Motions.Count);
            Assert.Empty(component.Actions);
            
            player.AllComponents.Add(component);
            
            Assert.Single(player.SadComponents);
            Assert.Single(player.GoRogueComponents);
        }
    }
}