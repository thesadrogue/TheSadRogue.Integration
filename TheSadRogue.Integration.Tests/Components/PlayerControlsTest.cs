using System.Collections.Generic;
using SadConsole.Input;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;
using Xunit;

namespace TheSadRogue.Integration.Tests.Components
{
    public class PlayerControlsTest
    {
        [Fact]
        public void NewDefaultPlayerControlsComponentTest()
        {
            var player = new RogueLikeEntity((0,0), 1, false, false, 1);
            var component = new PlayerControlsComponent();
            
            Assert.Equal(4, component.Motions.Count);
            Assert.Empty(component.Actions);
            
            player.AddComponent(component);

            Assert.Single(player.Components);
            // Assert.Single(player.SadComponents);
            // Assert.Single(player.GoRogueComponents);
        }

        [Fact]
        public void NewCustomPlayerControlsComponentTest()
        {
            var player = new RogueLikeEntity((0,0), 1, false, false, 1);
            var controls = new Dictionary<Keys, Direction>();
            controls.Add(Keys.W, Direction.Up);
            controls.Add(Keys.Q, Direction.UpLeft);
            controls.Add(Keys.A, Direction.Left);
            controls.Add(Keys.Z, Direction.DownLeft);
            controls.Add(Keys.X, Direction.Down);
            controls.Add(Keys.C, Direction.DownRight);
            controls.Add(Keys.D, Direction.Right);
            controls.Add(Keys.E, Direction.UpRight);
            controls.Add(Keys.S, Direction.None);
            
            var component = new PlayerControlsComponent(controls);
            
            Assert.Equal(9, component.Motions.Count);
            Assert.Empty(component.Actions);
            
            player.AddComponent(component);

            Assert.Single(player.Components);
        }
    }
}