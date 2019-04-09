using System.Collections.Generic;
using System.Text;
using DataModel.GalaxyModel;
using Navigation;
using UnityEngine;

namespace DataModel
{
    public class StarSystem
    {
        public string Name;
        public List<Planet> Planets;
        public Vector3Int Position;
        public float Diameter;
        public StarSystemConfiguration Configuration;

        public StarSystem(string name, Vector3Int position, float diameter)
        {
            Name = name;
            Position = position;
            Diameter = diameter;
            Planets = new List<Planet>();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            var belt = Configuration.HabitableBeltDistances;

            stringBuilder
                .AppendFormat("Starsystem Name: {0}\n", Name)
                .AppendFormat("Planets: {0}\n", Planets.Count)
                .AppendFormat("Position: {0}\n", Position)
                .AppendFormat("Diameter: {0} AU\n", Diameter)
                .AppendFormat("Habitable Belt: {0} AU to {1} AU\n", belt.Item1, belt.Item2);

            return stringBuilder.ToString();
        }

        public string ToDetailedString()
        {
            var stringBuilder = new StringBuilder();
            var belt = Configuration.HabitableBeltDistances;

            stringBuilder
                .Append(ToString());
            
            Planets.ForEach(planet => { stringBuilder.AppendLine(planet.ToString()); });

            return stringBuilder.ToString();
        }
    }
}