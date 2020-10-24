using SadRogue.Primitives;

namespace TheSadRogue.Integration.FieldOfView
{
    public class FieldOfViewHandler : IFieldOfViewHandler
    {
        public bool Enabled { get; private set; }
        public RogueLikeMap Map { get; }
        public FieldOfViewState State { get; private set; }
        public Color ExploredForegroundColor { get; }
        public Color ExploredBackgroundColor { get; }

        public FieldOfViewHandler(RogueLikeMap map, Color exploredForegroundColor, Color exploredBackgroundColor,
            FieldOfViewState state = FieldOfViewState.Enabled)
        {
            Map = map;
            ExploredForegroundColor = exploredForegroundColor;
            ExploredBackgroundColor = exploredBackgroundColor;
        }
        public void SetState(FieldOfViewState state)
        {
            throw new System.NotImplementedException();
        }

        public void Enable()
        {
            throw new System.NotImplementedException();
        }

        public void Disable(bool resetVisibility)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateTerrainSeen(RogueLikeEntity terrain)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateTerrainUnseen(RogueLikeEntity terrain)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateEntitySeen(RogueLikeEntity terrain)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateEntityUnseen(RogueLikeEntity terrain)
        {
            throw new System.NotImplementedException();
        }
    }
}