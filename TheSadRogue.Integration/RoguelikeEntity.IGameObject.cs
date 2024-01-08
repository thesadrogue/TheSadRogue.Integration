using System;
using GoRogue.Components;
using GoRogue.GameFramework;
using SadRogue.Primitives;

namespace SadRogue.Integration
{
    public partial class RogueLikeEntity
    {
        private bool _isTransparent;
        private bool _isWalkable;

        /// <inheritdoc />
        public uint ID { get; private set; }

        /// <inheritdoc />
        public int Layer => ZIndex;

        private Map? _currentMap;

        /// <inheritdoc />
        public Map? CurrentMap => _currentMap;

        /// <inheritdoc />
        public event EventHandler<GameObjectCurrentMapChanged>? AddedToMap;

        /// <inheritdoc />
        public event EventHandler<GameObjectCurrentMapChanged>? RemovedFromMap;

        // Nullable override to suppress warning on constructors; warning is incorrect; the functions that the
        // constructors call initializes this to a non-null value.
        /// <inheritdoc />
        public IComponentCollection GoRogueComponents { get; private set; } = null!;

        /// <inheritdoc />
        public bool IsTransparent
        {
            get => _isTransparent;
            set => this.SafelySetProperty(ref _isTransparent, value, TransparencyChanging, TransparencyChanged);
        }

        /// <inheritdoc />
        public bool IsWalkable
        {
            get => _isWalkable;
            set => this.SafelySetProperty(ref _isWalkable, value, WalkabilityChanging, WalkabilityChanged);
        }

        /// <inheritdoc />
        public void OnMapChanged(Map? newMap)
            => this.SafelySetCurrentMap(ref _currentMap, newMap, AddedToMap, RemovedFromMap);

        /// <inheritdoc />
        public event EventHandler<ValueChangedEventArgs<bool>>? TransparencyChanging;

        /// <inheritdoc />
        public event EventHandler<ValueChangedEventArgs<bool>>? TransparencyChanged;

        /// <inheritdoc />
        public event EventHandler<ValueChangedEventArgs<bool>>? WalkabilityChanging;

        /// <inheritdoc />
        public event EventHandler<ValueChangedEventArgs<bool>>? WalkabilityChanged;

        // TODO: Position is in danger here because the underlying field doesn't revert
    }
}
