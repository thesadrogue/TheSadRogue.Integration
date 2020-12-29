using System.Collections.Generic;
using System.Linq;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using GoRogue.SpatialMaps;
using SadConsole;
using SadConsole.Entities;
using SadRogue.Primitives;
using TheSadRogue.Integration.Extensions;

namespace TheSadRogue.Integration
{
    /// <summary>
    /// A Map that contains the necessary function to render to a ScreenSurface
    /// </summary>
    public class RogueLikeMap : Map
    {
        /// <summary>
        /// An IMapView of the ColoredGlyphs on the Terrain layer (0)
        /// </summary>
        public IMapView<ColoredGlyph> TerrainSurface
            => new LambdaTranslationMap<IGameObject, ColoredGlyph>(Terrain, val => ((RogueLikeEntity)val).Appearance);
        
        /// <summary>
        /// A hacky way to render the initial state of entities present. TODO - come up with a better way
        /// </summary>
        public IEnumerable<ICellSurface> Renderers => Entities.ToCellSurfaces(Width, Height);
        
        //public event EventHandler FieldOfViewRecalculated;
        //public IFieldOfViewHandler FovHandler;
        //public LayeredScreenSurface LayeredSurface;

        public EntityLiteManager EntityManager;
        
        
        #region constructors

        /// <summary>
        /// Creates a new RogueLikeMap
        /// </summary>
        /// <param name="width">Desired width of map</param>
        /// <param name="height">Desired Height of Map</param>
        /// <param name="numberOfEntityLayers">How many entity layers to include</param>
        /// <param name="distanceMeasurement">How to measure the distance of a single-tile movement</param>
        /// <param name="layersBlockingWalkability">Layers which should factor into move logic</param>
        /// <param name="layersBlockingTransparency">Layers which should factor into transparency</param>
        /// <param name="entityLayersSupportingMultipleItems">How many entity layers support multiple entities per layer</param>
        public RogueLikeMap(int width, int height, int numberOfEntityLayers, Distance distanceMeasurement,
            uint layersBlockingWalkability = 4294967295, uint layersBlockingTransparency = 4294967295,
            uint entityLayersSupportingMultipleItems = 0) : base(width, height, numberOfEntityLayers,
            distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency,
            entityLayersSupportingMultipleItems)
        {
            EntityManager = new EntityLiteManager();
            Entities.ItemAdded += Entity_Added;
        }

        private void Entity_Added(object? sender, ItemEventArgs<IGameObject> e)
        {
            if (EntityManager.Entities.Count(entity => entity == e.Item) == 0)
            {
                EntityManager.Add(e.Item as RogueLikeEntity);
            }
        }

        #endregion

        public void AddEntity(RogueLikeEntity entity)
        {
            EntityManager.Add(entity);
            
        }
    }
}