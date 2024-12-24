using System.Linq;
using GoRogue.GameFramework;
using SadRogue.Integration.Maps;
using SadRogue.Integration.Tests.Mocks;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using Xunit;

namespace SadRogue.Integration.Tests.FieldOfView;

public class FieldOfViewTests : IClassFixture<SadConsoleFixture>
{
    private const int Radius = 5;

    [Fact]
    public void FieldOfViewOnlyNewInSeen()
    {
        var pos = new Point(15, 15);
        var map = new RogueLikeMap(30, 30, null, 1, Distance.Manhattan);
        map.ApplyTerrainOverlay(
            new LambdaGridView<IGameObject>(map.Width, map.Height,
                _ => new RogueLikeCell(Color.White, Color.Black, '.', 0)));

        var fov = new CountingFieldOfViewHandler(false);
        map.AllComponents.Add(fov);

        map.PlayerFOV.Calculate(pos, Radius);

        Assert.Equal(map.Positions().Count(p => map.PlayerFOV.BooleanResultView[p]), fov.TerrainSeen);

        fov.ResetCounters();

        map.PlayerFOV.Calculate(16, 15, Radius);

        // Should be one vertical line on the right side new, and one old
        Assert.Equal(Radius * 2 + 1, fov.TerrainSeen);
        Assert.Equal(Radius * 2 + 1, fov.TerrainUnseen);
    }

    [Fact]
    public void FieldOfViewAllInSeen()
    {
        var pos = new Point(15, 15);
        var map = new RogueLikeMap(30, 30, null, 1, Distance.Manhattan);
        map.ApplyTerrainOverlay(
            new LambdaGridView<IGameObject>(map.Width, map.Height,
                _ => new RogueLikeCell(Color.White, Color.Black, '.', 0)));

        var fov = new CountingFieldOfViewHandler(true);
        map.AllComponents.Add(fov);

        map.PlayerFOV.Calculate(pos, Radius);

        Assert.Equal(map.Positions().Count(p => map.PlayerFOV.BooleanResultView[p]), fov.TerrainSeen);

        fov.ResetCounters();

        map.PlayerFOV.Calculate(16, 15, Radius);

        // Should be the entire FOV in seen, and one vertical line in old
        Assert.Equal(map.Positions().Count(p => map.PlayerFOV.BooleanResultView[p]), fov.TerrainSeen);
        Assert.Equal(Radius * 2 + 1, fov.TerrainUnseen);
    }

}
