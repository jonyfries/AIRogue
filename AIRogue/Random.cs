using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace AIRogue
{
    static class Random
    {
        static private System.Random random = new System.Random();

        static public int Next()
        {
            return random.Next();
        }

        static public int Next(int maxValue)
        {
            return random.Next(maxValue);
        }

        static public int Next(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        static public Point Next(Point maxValues)
        {
            return new Point(random.Next(maxValues.X), random.Next(maxValues.Y));
        }

        static public Point Next(Point minValues, Point maxValues)
        {
            return new Point(random.Next(minValues.X, maxValues.X), random.Next(minValues.Y, maxValues.Y));
        }

        static public T Choice<T>(List<T> itemList)
        {
            int index = random.Next(itemList.Count);
            return itemList[index];
        }

        static public T EnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(random.Next(v.Length));
        }
    }
}
