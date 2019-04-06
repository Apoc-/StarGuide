using UnityEngine;
using Random = System.Random;

namespace Util
{
    public static class MathHelper
    {
        private static Random _random = new Random();

        /// <summary>
        /// Returns a random int between min (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomInt(int min, int max)
        {
            return _random.Next(min, max);
        }
        
        /// <summary>
        /// Returns a random int between 0 (inclusive) and max (exclusive).
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomInt(int max)
        {
            return _random.Next(max);
        }
        
    }
}