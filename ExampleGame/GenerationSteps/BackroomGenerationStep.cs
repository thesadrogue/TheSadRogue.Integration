using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadRogue.Primitives;
using SFML.Window;

namespace ExampleGame
{
    public class BackroomGenerationStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            Random random = new Random();
            var map = context.GetFirst<ISettableMapView<bool>>();
            var rooms = new List<Rectangle>();
            var largeRooms = new List<Rectangle>();
            int thirdWidth = map.Width / 3;
            int halfWidth = map.Width / 2;
            int thirdHeight = map.Height / 3;
            int halfHeight = map.Height / 2; 
            
            if (random.Next(0, 2) % 2 == 0)
            {
                if (random.Next(0, 2) % 2 == 0)
                {
                    largeRooms.Add(new Rectangle(halfWidth, 0, halfWidth, map.Height));
                    largeRooms.Add(new Rectangle(0, 0, halfWidth, thirdHeight));
                    largeRooms.Add(new Rectangle(0, thirdHeight, halfWidth, thirdHeight));
                    largeRooms.Add(new Rectangle(0, thirdHeight * 2, halfWidth, thirdHeight));
                }
                else
                {
                    largeRooms.Add(new Rectangle(0, 0, halfWidth, map.Height));
                    largeRooms.Add(new Rectangle(halfWidth, 0, halfWidth, thirdHeight));
                    largeRooms.Add(new Rectangle(halfWidth, thirdHeight, halfWidth, thirdHeight));
                    largeRooms.Add(new Rectangle(halfWidth, thirdHeight * 2, halfWidth, thirdHeight));
                }
            }
            else
            {
                if (random.Next(0, 2) % 2 == 0)
                {
                    largeRooms.Add(new Rectangle(0,0, map.Width, halfHeight));
                    largeRooms.Add(new Rectangle(0, halfHeight, thirdWidth, halfHeight));
                    largeRooms.Add(new Rectangle(thirdWidth, halfHeight, thirdWidth, halfHeight));
                    largeRooms.Add(new Rectangle(thirdWidth * 2, halfHeight, thirdWidth, halfHeight));
                }
                else
                {
                    largeRooms.Add(new Rectangle(0,halfHeight, map.Width, halfHeight));
                    largeRooms.Add(new Rectangle(0, 0, thirdWidth, halfHeight));
                    largeRooms.Add(new Rectangle(thirdWidth, 0, thirdWidth, halfHeight));
                    largeRooms.Add(new Rectangle(thirdWidth * 2, 0, thirdWidth, halfHeight));
                }
            }

            foreach(var rectangle in largeRooms)
                rooms.AddRange(rectangle.BisectRecursive(random.Next(3,9)));

            foreach (var room in rooms)
                foreach (var point in room.PerimeterPositions())
                    map[point] = false;
            
            yield return null;
        }
    }
}