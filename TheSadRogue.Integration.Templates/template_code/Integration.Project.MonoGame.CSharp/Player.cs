using GoRogue.GameFramework;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;
using SadRogue.Primitives;

namespace TheSadRogue.Integration.Templates.MonoGame
{
    internal class Player : RogueLikeEntity
    {
        /// <summary>
        /// The sight radius of the player.
        /// </summary>
        public int FOVRadius { get; init; } = 10;

        public Player(Point position)
            : base(position, '@', false)
        {
            // Set hook so that FOV is recalculated when the player moves
            Moved += OnMoved;

            // Add component for controlling player movement via keyboard
            var motionControl = new PlayerKeybindingsComponent();
            motionControl.SetMotions(PlayerKeybindingsComponent.ArrowMotions);
            AllComponents.Add(motionControl);
        }

        /// <summary>
        /// Calculate FOV if a player is part of a map.
        /// </summary>
        public void CalculateFOV()
        {
            CurrentMap?.PlayerFOV.Calculate(Position, FOVRadius, CurrentMap.DistanceMeasurement);
        }

        // If the player is added to a map, update the player FOV when the player moves
        private void OnMoved(object sender, GameObjectPropertyChanged<Point> e)
        {
            CalculateFOV();
        }

    }
}
