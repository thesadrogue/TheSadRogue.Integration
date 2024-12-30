using SadRogue.Integration.FieldOfView;

namespace SadRogue.Integration.Tests.Mocks;

/// <summary>
/// An FOV handler only useful for testing which simply counts the number of times its functions are called.
/// </summary>
class CountingFieldOfViewHandler : FieldOfViewHandlerBase
{
    /// <summary>
    /// Number of times <see cref="UpdateTerrainSeen"/> was called.
    /// </summary>
    public int TerrainSeen { get; private set; }

    /// <summary>
    /// Number of times <see cref="UpdateTerrainUnseen"/> was called.
    /// </summary>
    public int TerrainUnseen { get; private set; }

    /// <summary>
    /// Number of times <see cref="UpdateEntitySeen"/> was called.
    /// </summary>
    public int EntitySeen { get; private set; }

    /// <summary>
    /// Number of times <see cref="UpdateEntityUnseen"/> was called.
    /// </summary>
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

    /// <summary>
    /// Reset all the function counters to 0.
    /// </summary>
    public void ResetCounters()
    {
        TerrainSeen = 0;
        TerrainUnseen = 0;
        EntitySeen = 0;
        EntityUnseen = 0;
    }
}
