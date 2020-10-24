using System.Collections.Generic;
using System.Linq;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using SadConsole;
using SadRogue.Primitives;

namespace TheSadRogue.Integration
{
    public class RogueLikeMap : Map
    {
        public IMapView<RogueLikeEntity> IntegratedTerrain
            => new LambdaTranslationMap<IGameObject, RogueLikeEntity>(Terrain, val => (RogueLikeEntity)val); 
        public IMapView<ColoredGlyph> GlyphMap
            => new LambdaTranslationMap<IGameObject, ColoredGlyph>(Terrain, val => ((RogueLikeEntity)val).Glyph); 

        public ColoredGlyph[] RenderingCellData => GlyphMap.ToArray();

        #region constructors
        public RogueLikeMap(int width, int height, int numberOfEntityLayers, Distance distanceMeasurement, uint layersBlockingWalkability = 4294967295, uint layersBlockingTransparency = 4294967295, uint entityLayersSupportingMultipleItems = 0) : base(width, height, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency, entityLayersSupportingMultipleItems)
        {
            Init(width, height);
        }

        public RogueLikeMap(ISettableMapView<IGameObject?> terrainLayer, int numberOfEntityLayers, Distance distanceMeasurement, uint layersBlockingWalkability = 4294967295, uint layersBlockingTransparency = 4294967295, uint entityLayersSupportingMultipleItems = 0) : base(terrainLayer, numberOfEntityLayers, distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency, entityLayersSupportingMultipleItems)
        {
            Init(terrainLayer.Width, terrainLayer.Height);
        }
        #endregion

        public void Init(int width, int height)
        {
        }
    }
}