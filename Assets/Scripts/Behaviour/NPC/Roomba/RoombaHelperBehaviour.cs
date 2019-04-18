using CreativeSpore.SuperTilemapEditor;
using UnityEngine;

namespace Behaviour.NPC.Roomba
{
    [ExecuteInEditMode]
    public class RoombaHelperBehaviour: MonoBehaviour
    {
        public void OnTilePrefabCreation(TilemapChunk.OnTilePrefabCreationData data)
        {
            var db = GetComponentInChildren<RoombaBehaviour>();
            db.ParentTilemapGroup = data.ParentTilemap.ParentTilemapGroup;
            
            db.name = $"Roomba {new Vector2(data.GridX, data.GridY)}";
        }
    }
}