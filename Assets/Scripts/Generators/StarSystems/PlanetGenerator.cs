using System;
using System.Linq;
using DataModel;
using DataModel.GalaxyModel;
using Navigation;
using Util;

namespace UnityEngine
{
    public class PlanetGenerator
    {
        private StarSystem _system;
        
        public PlanetGenerator(StarSystem system)
        {
            _system = system;
        }

        public Planet GeneratePlanet(PlanetSize size)
        {
            var position = GetRandomPosition();
            var category = GetRandomCategory(position);
            var planet = new Planet(position, size, category);
            
            return planet;
        }

        private PlanetCategory GetRandomCategory(PolarPosition position)
        {
            var beltMin = _system.Configuration.HabitableBeltDistances.Item1;
            var beltMax = _system.Configuration.HabitableBeltDistances.Item2;
            var beltDist = beltMax - beltMin;

            var habMin = beltMin + beltDist * (1/3f);
            var habMax = beltMin + beltDist * (2/3f);

            var planetDist = position.Radius;

            if (planetDist < beltMin || planetDist > beltMax)
            {
                return PlanetCategory.Hell;
            }

            if (planetDist > habMin && planetDist < habMax)
            {
                return PlanetCategory.Habitable;
            }

            return PlanetCategory.Harsh;
        }

        private PolarPosition GetRandomPosition()
        {
            var dia = _system.Diameter;

            var x = MathHelper.GetRandomFloat(0, dia);
            var y = MathHelper.GetRandomFloat(0, 2*Mathf.PI);
            
            return new PolarPosition(x,y);
        }
    }
}