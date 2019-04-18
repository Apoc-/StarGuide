using CreativeSpore.SuperTilemapEditor;
using UnityEngine;

namespace Behaviour.World.Door
{
    [ExecuteInEditMode]
    public class DoorHelperBehaviour : MonoBehaviour
    {
        public void OnTilePrefabCreation(TilemapChunk.OnTilePrefabCreationData data)
        {
            var db = GetComponent<DoorBehaviour>();
            db.DoorLocation = new Vector2(data.GridX, data.GridY);
            db.DoorTilemap = data.ParentTilemap.ParentTilemapGroup.FindTilemapByName("Doors");
            db.name = $"Door {db.DoorLocation}";
        }
    }
}