using Navigation;
using UnityEngine;

namespace DataModel
{
    public class SpaceShipData
    {
        public string Name;
        public Vector3Int GridPosition { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Heading { get; set; } = Vector3.zero;
        public ShipEngine Engine;

        public string GetNavigationDataString()
        {
            var dataString = "";
            dataString += $"Pos: ({Position.x:F3}, {Position.y:F3}, {Position.z:F3})\n";
            dataString += $"Grid: {GridPosition}\n";
            dataString += $"Heading: ({Heading.x:F3}, {Heading.y:F3}, {Heading.z:F3})\n";
            return dataString;
        }
    }
}