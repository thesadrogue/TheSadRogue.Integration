using GoRogue.MapViews;

namespace TheSadRogue.Integration.Extensions
{
    public static class IMapViewExtensions
    {
        /// <summary>
        /// Converts the given mapview into an array of T[]
        /// </summary>
        /// <param name="self">the map view being transformed</param>
        /// <typeparam name="T">The type of which to return an array</typeparam>
        /// <returns></returns>
        public static T[] ToArray<T>(this IMapView<T> self)
        {
            var array = new ArrayMap<T>(self.Width, self.Height);
            array.ApplyOverlay(self);
            return array;
        }
    }
}