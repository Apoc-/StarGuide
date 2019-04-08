using System.Text;
using Navigation;
using UnityEngine;

namespace DataModel
{
    public class Planet
    {
        public string Name;
        public PolarPosition Position;
        public PlanetSize Size;
        public PlanetCategory Category;

        public Planet(PolarPosition position, PlanetSize size, PlanetCategory category)
        {
            Position = position;
            Size = size;
            Category = category;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder
                .AppendFormat("Planet Name: {0}\n", Name)
                .AppendFormat("Position: {0}\n", Position)
                .AppendFormat("Size: {0}\n", Size)
                .AppendFormat("Category: {0}\n", Category);

            return stringBuilder.ToString();
        }
    }
}