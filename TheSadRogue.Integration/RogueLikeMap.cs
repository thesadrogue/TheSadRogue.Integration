using System;
using System.Collections.Generic;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadConsole;
using SadRogue.Primitives;
using TheSadRogue.Integration.FieldOfView;

namespace TheSadRogue.Integration
{
    public class RogueLikeMap : Map
    {
        public IMapView<RogueLikeEntity> RogueLikeTerrain
            => new LambdaTranslationMap<IGameObject, RogueLikeEntity>(Terrain, val => (RogueLikeEntity)val); 
        public IMapView<ColoredGlyph> TerrainSurface
            => new LambdaTranslationMap<IGameObject, ColoredGlyph>(Terrain, val => ((RogueLikeEntity)val).Glyph);
        public IEnumerable<Region> Regions => _regions;
        public ICellSurface EntitySurface { get; }
        
        public IEnumerable<ICellSurface> RenderingGlyphs { get; private set; }
        public event EventHandler FieldOfViewRecalculated;
        public IFieldOfViewHandler FovHandler;
        private IEnumerable<Region> _regions;

        #region constructors
        public RogueLikeMap(int width, int height, IFieldOfViewHandler fovHandler, Distance distanceMeasurement) : this(
            width, height, 31, distanceMeasurement)
        {
            FovHandler = fovHandler;
            Init(width, height);
        }
        public RogueLikeMap(int width, int height, int numberOfEntityLayers, Distance distanceMeasurement,
            uint layersBlockingWalkability = 4294967295, uint layersBlockingTransparency = 4294967295,
            uint entityLayersSupportingMultipleItems = 0) : base(width, height, numberOfEntityLayers,
            distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency,
            entityLayersSupportingMultipleItems)
        {
            Init(width, height);
        }

        public RogueLikeMap(ISettableMapView<IGameObject?> terrainLayer, int numberOfEntityLayers, Distance distanceMeasurement, uint layersBlockingWalkability = 4294967295, uint layersBlockingTransparency = 4294967295, uint entityLayersSupportingMultipleItems = 0) : base(terrainLayer, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency, entityLayersSupportingMultipleItems)
        {
            Init(terrainLayer.Width, terrainLayer.Height);
        }
        public void Init(int width, int height)
        {
            _regions = new List<Region>();
            RenderingGlyphs = new List<ICellSurface>();
        }
        #endregion
    }
}