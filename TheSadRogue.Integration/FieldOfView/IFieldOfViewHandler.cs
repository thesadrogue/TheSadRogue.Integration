using SadRogue.Primitives;

namespace TheSadRogue.Integration.FieldOfView
{
    public interface IFieldOfViewHandler
    {
        bool Enabled { get; }
        RogueLikeMap Map { get; }
        FieldOfViewState State { get; }
        Color ExploredForegroundColor { get; }
        Color ExploredBackgroundColor { get; }
        void SetState(FieldOfViewState state);
        void Enable();
        void Disable(bool resetVisibility);
        void UpdateTerrainSeen(RogueLikeEntity terrain);
        void UpdateTerrainUnseen(RogueLikeEntity terrain);
        void UpdateEntitySeen(RogueLikeEntity terrain);
        void UpdateEntityUnseen(RogueLikeEntity terrain);
    }
}