using System.Linq;
using UnityEngine;

namespace Behaviour.World
{
    public class ChairInteractible : Interactible
    {
        public Vector2 LeftTilePosition;
        public Vector2 RightTilePosition;
        
        public override void OnInteract(PlayerInputController playerInputController)
        {
            playerInputController.SitDown(this);
            DisplayBackrest();
        }

        private void DisplayBackrest()
        {
            var map = GameManager.Instance
                .ShipTilemapGroup
                .Tilemaps
                .First(tilemap => tilemap.name == "DoodadsForeground");
            
            map.SetTile(LeftTilePosition, 200);
            map.SetTile(RightTilePosition, 201);
        }

        public void HideBackrest()
        {
            var map = GameManager.Instance
                .ShipTilemapGroup
                .Tilemaps
                .First(tilemap => tilemap.name == "DoodadsForeground");

            map.Erase(LeftTilePosition);
            map.Erase(RightTilePosition);
            
            map.UpdateMesh();
        }
    }
}