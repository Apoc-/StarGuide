using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Util
{
    public static class ListHelper
    {
        public static IList<T> Shuffle<T>(this IList<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;
                int k = MathHelper.GetRandomInt(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }

            return list;
        }
        
        // Ex: collection.TakeLast(5);
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }
    }
}