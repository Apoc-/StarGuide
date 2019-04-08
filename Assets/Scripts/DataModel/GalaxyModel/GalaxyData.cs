using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DataModel
{
    public class GalaxyData
    {
        public Dictionary<Vector3, StarSystem> StarSystems;
        public string Name;

        public GalaxyData()
        {
            StarSystems = new Dictionary<Vector3, StarSystem>();
        }
        
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder
                .AppendFormat("Galaxy: {0}\n", Name)
                .AppendFormat("Starsystems: {0}\n", StarSystems.Count)
                .AppendLine("-----------");
            
            foreach (var starSystem in StarSystems.Values)
            {
                stringBuilder.AppendLine(starSystem.ToString());
            }

            return stringBuilder.ToString();
        }
    }
}