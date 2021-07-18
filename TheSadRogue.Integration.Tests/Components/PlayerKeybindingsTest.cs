using SadRogue.Primitives;
using SadRogue.Integration.Components.Keybindings;
using Xunit;

namespace SadRogue.Integration.Tests.Components
{
    public class PlayerControlsTest
    {
        [Fact]
        public void NewPlayerKeybindingsComponent()
        {
            var player = new RogueLikeEntity((0,0), Color.White,1);
            var component = new PlayerKeybindingsComponent();

            Assert.Equal(4, component.Motions.Count);
            Assert.Empty(component.Actions);

            player.AllComponents.Add(component);

            Assert.Single(player.SadComponents);
            Assert.Single(player.GoRogueComponents);
        }
    }
}
