using System.Collections.Generic;
using Assets.Scripts;
using CreativeSpore.SuperTilemapEditor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorBehaviour : MonoBehaviour
{
    private Collider2D _doorTrigger;
    private bool _oldState = true;

    private readonly HashSet<Collider2D> CollidersTouchingMe = new HashSet<Collider2D>();
    public Vector2 DoorLocation;
    public STETilemap DoorTilemap;

    public int WallTileId = 40;
    public int FloorTileId = 72;

    public int LockedWallTileId = 39;
    public int LockedFloorTileId = 71;

    public int OpenWallTileId = 41;
    public int OpenFloorTileId = 73;

    public bool Locked = false;

    private bool IsOpen => CollidersTouchingMe.Count > 0;

    private void Start()
    {
        _doorTrigger = GetComponent<Collider2D>();
        
        DoorTilemap.ParentTilemapGroup
            .FindTilemapByName("ShipForeground")
            .SetTile(DoorLocation, 11);
    }

    // Destroy everything that enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        var canOpenDoors = other.gameObject.GetComponent<ICanOpenDoors>() != null;
        if (canOpenDoors && !CollidersTouchingMe.Contains(other)) CollidersTouchingMe.Add(other);
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (CollidersTouchingMe.Contains(other)) CollidersTouchingMe.Remove(other);
    }

    private void Update()
    {
        if (!Application.isPlaying) return;

        SetDoorState(IsOpen);
    }

    private void SetDoorState(bool state)
    {
        if (state == _oldState)
            return;

        var top = DoorLocation;
        var bot = new Vector2(top.x, top.y + 1);

        var tileTop = WallTileId; // Default is closed
        var tileBot = FloorTileId;

        if (Locked)
        {
            tileTop = LockedWallTileId;
            tileBot = LockedFloorTileId;
        }
        else if (state)
        {
            tileTop = OpenWallTileId;
            tileBot = OpenFloorTileId;
        }

        DoorTilemap.SetTile(bot, tileTop);
        DoorTilemap.SetTile(top, tileBot);

        _oldState = state;
        DoorTilemap.UpdateMeshImmediate();
    }
}