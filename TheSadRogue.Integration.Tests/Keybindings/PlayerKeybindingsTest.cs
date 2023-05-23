using SadRogue.Integration.Keybindings;
using SadRogue.Primitives;
using Xunit;

namespace SadRogue.Integration.Tests.Keybindings
{
    public class PlayerControlsTest
    {
        [Fact]
        public void NewPlayerKeybindingsComponent()
        {
            var player = new RogueLikeEntity((0,0), Color.White,1);
            var component = new KeybindingsComponent();

            Assert.Empty(component.Motions);
            Assert.Empty(component.Actions);

            player.AllComponents.Add(component);

            Assert.Single(player.SadComponents);
            Assert.Single(player.GoRogueComponents);
        }
    }
}
