using System.Linq;
using Behaviour.World.Door;
using CreativeSpore.SuperTilemapEditor;
using UnityEngine;

namespace Behaviour.World
{
    [ExecuteAlways]
    public class ChairInteractible : Interactible
    {
        public Vector2 LeftTilePosition;
        public Vector2 RightTilePosition;

        public STETilemap DoodadsForeground;
        public STETilemap DoodadsBackground;

        public override void OnInteract(PlayerInputController playerInputController)
        {
            playerInputController.SitDown(this);
            DisplayBackrest();
        }

        public void OnTilePrefabCreation(TilemapChunk.OnTilePrefabCreationData data)
        {
            Debug.Assert(data.ParentTilemap.name.Equals("DoodadsBackground"), "Captains chair must be on DoodadsBackground");
            
            LeftTilePosition = new Vector2(data.GridX, data.GridY);
            RightTilePosition = new Vector2(data.GridX + 1, data.GridY);
            DoodadsForeground = data.ParentTilemap.ParentTilemapGroup.FindTilemapByName("DoodadsForeground");
            DoodadsBackground = data.ParentTilemap.ParentTilemapGroup.FindTilemapByName("DoodadsBackground");
            gameObject.name = $"CptnChair\n{LeftTilePosition}";
        }

        private void DisplayBackrest()
        {
            var map = DoodadsForeground;

            map.SetTile(LeftTilePosition, 200);
            map.SetTile(RightTilePosition, 201);

            map.UpdateMesh();
        }

        public void HideBackrest()
        {
            var map = DoodadsForeground;

            map.Erase(LeftTilePosition);
            map.Erase(RightTilePosition);

            map.UpdateMesh();
        }
    }
}