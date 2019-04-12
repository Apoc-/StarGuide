using System.Collections;
using System.Collections.Generic;
using Behaviour;
using CreativeSpore.SuperTilemapEditor;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class DoorHelperBehaviour : MonoBehaviour
{
    public void OnTilePrefabCreation(TilemapChunk.OnTilePrefabCreationData data)
    {
        var db = GetComponent<DoorBehaviour>();
        db.DoorLocation = new Vector2(data.GridX, data.GridY);
        db.DoorTilemap = data.ParentTilemap.ParentTilemapGroup.FindTilemapByName("Doors");
        db.name = $"Door {db.DoorLocation}";
        
        EditorUtility.SetDirty(db);
    }
}