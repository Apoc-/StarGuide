using System;
using System.Collections.Generic;

namespace DataModel.GalaxyModel
{
    public class StarSystemConfiguration
    {
        public Dictionary<PlanetSize, int> PlanetSizes { get; }
        public Tuple<float, float> HabitableBeltDistances { get; }
        public StarSystemConfiguration(Dictionary<PlanetSize, int> planetSizes, Tuple<float, float> habitableBeltDistances)
        {
            PlanetSizes = planetSizes;
            HabitableBeltDistances = habitableBeltDistances;
        }
    }
}