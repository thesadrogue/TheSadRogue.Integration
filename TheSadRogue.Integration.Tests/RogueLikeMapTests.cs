using SadRogue.Primitives;
using Xunit;

namespace TheSadRogue.Integration.Tests
{
    public class RogueLikeMapTests
    {
        [Fact]
        public void NewMapTest()
        {
            var map = new RogueLikeMap(24, 12, 3, Distance.Chebyshev);
            Assert.Equal(24, map.Width);
            Assert.Equal(12, map.Height);
            Assert.Equal(3, map.Entities.NumberOfLayers);
            Assert.Equal(Distance.Chebyshev, map.DistanceMeasurement);
            Assert.Equal(24, map.Width);
        }

        // [Fact]
        // public void EntityAddedTest()
        // {
        //     var map = new RogueLikeMap(24, 12, 3, Distance.Chebyshev);
        //     var surface = new ScreenSurface(10, 10);
        //     map.SetEntitySurface(surface);
        //     
        //     Assert.Empty(surface.Children);
        // }
    }
}