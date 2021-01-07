using System;
using System.Collections;
using System.Collections.Generic;
using GoRogue.GameFramework;
using SadConsole;
using SadConsole.Effects;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace TheSadRogue.Integration
{
    /// <summary>
    /// A CellSurface that renders the terrain layer of a map.
    /// </summary>
    public class SettableCellSurface : GridViewBase<ColoredGlyph?>, ICellSurface
    {
        private bool _isDirty = true;
        private readonly BoundedRectangle _viewArea;
        private Color _defaultBackground;
        private Color _defaultForeground;
        private Map _map;
     
        #region properties
        /// <inheritdoc />
        public override int Height => _viewArea.BoundingBox.Height;
        
        /// <inheritdoc />
        public override int Width => _viewArea.BoundingBox.Width;

        /// <inheritdoc />
        public override ColoredGlyph? this[Point pos] => _map.GetTerrainAt<RogueLikeCell>(pos)?.Appearance;
        
        /// <inheritdoc />
        public Rectangle Area => _viewArea.BoundingBox;
        
        /// <inheritdoc />
        public int TimesShiftedDown { get; set; }
        
        /// <inheritdoc />
        public int TimesShiftedRight { get; set; }
        
        /// <inheritdoc />
        public int TimesShiftedLeft { get; set; }
        
        /// <inheritdoc />
        public int TimesShiftedUp { get; set; }
        
        /// <inheritdoc />
        public bool UsePrintProcessor { get; set; }
                
        /// <inheritdoc />
        public EffectsManager Effects { get; }
        
        /// <inheritdoc />
        public Color DefaultBackground 
        {
            get => _defaultForeground;
            set { _defaultForeground = value; IsDirty = true; }
        }
        /// <inheritdoc />
        public Color DefaultForeground
        {
            get => _defaultBackground;
            set { _defaultBackground = value; IsDirty = true; }
        }
        
        /// <inheritdoc />
        public int DefaultGlyph { get; set; }
        
        /// <inheritdoc />
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    IsDirtyChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        
        /// <inheritdoc />
        public bool IsScrollable => Height != _viewArea.Area.Height || Width != _viewArea.Area.Width;
        
        /// <inheritdoc />
        public Rectangle View 
        {
            get => _viewArea.Area;
            set
            {
                _viewArea.SetArea(value);
                IsDirty = true;
            }
        }
        
        /// <inheritdoc />
        public int ViewHeight
        {
            get => _viewArea.Area.Height;
            set => _viewArea.SetArea(_viewArea.Area.WithHeight(value));
        }
        
        /// <inheritdoc />
        public Point ViewPosition
        {
            get => _viewArea.Area.Position;
            set
            {
                _viewArea.SetArea(_viewArea.Area.WithPosition(value));
                IsDirty = true;
            }
        }
        
        /// <inheritdoc />
        public int ViewWidth
        {
            get => _viewArea.Area.Width;
            set => _viewArea.SetArea(_viewArea.Area.WithWidth(value));
        }
        
        /// <inheritdoc />
        public event EventHandler IsDirtyChanged;
        #endregion
        
        /// <summary>
        /// Create a new SettableCellSurface
        /// </summary>
        /// <param name="map">The map which we are rendering</param>
        /// <param name="viewWidth">The height of the view (screen size)</param>
        /// <param name="viewHeight">The Width of the view (screen size)</param>
        public SettableCellSurface(RogueLikeMap map, int viewWidth, int viewHeight)
        {
            _map = map;
            Effects = new EffectsManager(this);
            
            _viewArea = new BoundedRectangle((0, 0, viewWidth, viewHeight),
                (0, 0, map.Width, map.Height));
        }
        
        /// <inheritdoc />
        public IEnumerator<ColoredGlyph> GetEnumerator()
        {
            foreach (var pos in this.Positions())
                yield return this[pos];
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public void Resize(int width, int height, int bufferWidth, int bufferHeight, bool clear)
            => throw new NotSupportedException($"Surfaces representing a {nameof(RogueLikeMap)} may not be resized.");

        /// <inheritdoc />
        public ICellSurface GetSubSurface(Rectangle view)
            => throw new NotSupportedException($"Surfaces representing a {nameof(RogueLikeMap)} cannot have subsurfaces created from them currently.");

        /// <inheritdoc />
        public void SetSurface(in ICellSurface surface, Rectangle view = new Rectangle())
            => throw new NotSupportedException($"Surfaces representing a {nameof(RogueLikeMap)} do not support SetSurface operations.");

        /// <inheritdoc />
        public void SetSurface(in ColoredGlyph[] cells, int width, int height, int bufferWidth, int bufferHeight)
            => throw new NotSupportedException($"Surfaces representing a {nameof(RogueLikeMap)} do not support SetSurface operations.");
    }
}