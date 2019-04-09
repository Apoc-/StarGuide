using Navigation;
using UnityEngine;

namespace DataModel
{
    public class SpaceShip
    {
        public string Name;
        public Vector3Int Position { get; set; }
        public ShipEngine Engine;
    }
}