using System;
using System.Collections.Generic;
using System.Linq;
using DataModel;
using DataModel.GalaxyModel;
using Navigation;
using Util;

namespace UnityEngine
{
    public class StarSystemGenerator
    {
        private GalaxyData _galaxyData;

        public StarSystemGenerator(GalaxyData galaxyData)
        {
            _galaxyData = galaxyData;
        }

        public StarSystem GenerateStarSystem(Vector3Int position)
        {
            var name = "Star System " + _galaxyData.StarSystems.Count;
            var diameter = GetRandomDiameter();
            var system = new StarSystem(name, position, diameter);

            system.Configuration = GenerateStarSystemConfiguration(system);
            var planetGen = new PlanetGenerator(system);

            system.Configuration = GenerateStarSystemConfiguration(system);
            foreach (var pair in system.Configuration.PlanetSizes)
            {
                var count = pair.Value;

                for (int i = 0; i < count; i++)
                {
                    var planet = planetGen.GeneratePlanet(pair.Key);
                    system.Planets.Add(planet);
                }
            }

            SortPlanetsByDistance(system);
            GeneratePlanetNames(system);

            return system;
        }

        private void GeneratePlanetNames(StarSystem system)
        {
            for (var i = 0; i < system.Planets.Count; i++)
            {
                system.Planets[i].Name = system.Name + "-" + i+1;
            }
        }

        private void SortPlanetsByDistance(StarSystem system)
        {
            system.Planets.Sort((planet, planet1) =>
            {
                return planet.Position.Radius.CompareTo(planet1.Position.Radius);
            });
        }

        private StarSystemConfiguration GenerateStarSystemConfiguration(StarSystem system)
        {
            var sizes = new Dictionary<PlanetSize, int>
            {
                [PlanetSize.Small] = MathHelper.GetRandomInt(3, 21),
                [PlanetSize.Medium] = MathHelper.GetRandomInt(1, 5),
                [PlanetSize.Large] = MathHelper.GetRandomInt(0, 3),
                [PlanetSize.Gigantic] = MathHelper.GetRandomInt(0, 1)
            };

            var minDist = MathHelper.GetRandomFloat(system.Diameter * 0.4f, system.Diameter * 0.45f);
            var maxDist = MathHelper.GetRandomFloat(system.Diameter * 0.55f, system.Diameter * 0.6f);
            
            return new StarSystemConfiguration(sizes, new Tuple<float, float>(minDist, maxDist));
        }

        private float GetRandomDiameter()
        {
            return MathHelper.GetRandomFloat(30000f, 300000f);
        }
    }
}