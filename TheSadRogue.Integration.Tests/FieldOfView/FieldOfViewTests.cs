using System.Linq;
using GoRogue.GameFramework;
using SadConsole.Quick;
using SadRogue.Integration.FieldOfView;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using Xunit;

namespace SadRogue.Integration.Tests.FieldOfView;

class CountingFieldOfViewHandler : FieldOfViewHandlerBase
{
    public int TerrainSeen { get; private set; }
    public int TerrainUnseen { get; private set; }

    public int EntitySeen { get; private set; }
    public int EntityUnseen { get; private set; }

    public CountingFieldOfViewHandler(bool newlySeenIncludesAllSeen)
        : base(newlySeenIncludesAllSeen: newlySeenIncludesAllSeen)
    {
    }

    protected override void UpdateTerrainSeen(RogueLikeCell terrain)
    {
        TerrainSeen++;
    }

    protected override void UpdateTerrainUnseen(RogueLikeCell terrain)
    {
        TerrainUnseen++;
    }

    protected override void UpdateEntitySeen(RogueLikeEntity entity)
    {
        EntitySeen++;
    }

    protected override void UpdateEntityUnseen(RogueLikeEntity entity)
    {
        EntityUnseen++;
    }

    public void ResetCounters()
    {
        TerrainSeen = 0;
        TerrainUnseen = 0;
        EntitySeen = 0;
        EntityUnseen = 0;
    }
}

public class FieldOfViewTests : IClassFixture<SadConsoleFixture>
{
    private const int RADIUS = 5;

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

        map.PlayerFOV.Calculate(pos, RADIUS);

        Assert.Equal(map.Positions().Count(p => map.PlayerFOV.BooleanResultView[p]), fov.TerrainSeen);

        fov.ResetCounters();

        map.PlayerFOV.Calculate(16, 15, RADIUS);

        // Should be one vertical line on the right side new, and one old
        Assert.Equal(RADIUS * 2 + 1, fov.TerrainSeen);
        Assert.Equal(RADIUS * 2 + 1, fov.TerrainUnseen);
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

        map.PlayerFOV.Calculate(pos, RADIUS);

        Assert.Equal(map.Positions().Count(p => map.PlayerFOV.BooleanResultView[p]), fov.TerrainSeen);

        fov.ResetCounters();

        map.PlayerFOV.Calculate(16, 15, RADIUS);

        // Should be the entire FOV in seen, and one vertical line in old
        Assert.Equal(map.Positions().Count(p => map.PlayerFOV.BooleanResultView[p]), fov.TerrainSeen);
        Assert.Equal(RADIUS * 2 + 1, fov.TerrainUnseen);
    }

}
