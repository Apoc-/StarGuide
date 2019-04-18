using System.Collections.Generic;
using Assets.Scripts;
using CreativeSpore.SuperTilemapEditor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorBehaviour : MonoBehaviour
{
    private readonly HashSet<Collider2D> CollidersTouchingMe = new HashSet<Collider2D>();
    private Collider2D _doorTrigger;
    private bool _oldState = true;

    [Header("These values are set by the tilemap.")]
    public Vector2 DoorLocation;

    public STETilemap DoorTilemap;

    
    public bool Locked;

    [Header("The tile IDs for the sprites that should be placed.")]
    public int DoorHolderTile = 11;
    [Space(10)]
    public int LockedFloorTileId = 71;
    public int LockedWallTileId = 39;
    [Space(10)]
    public int OpenFloorTileId = 73;
    public int OpenWallTileId = 41;
    [Space(10)]
    public int FloorTileId = 72;
    public int WallTileId = 40;

    [Header("Used to determine whether the sprites must be mirrored or not.")]
    public bool InvertTiles;

    public bool IsOpen => CollidersTouchingMe.Count > 0;

    private void Start()
    {
        _doorTrigger = GetComponent<Collider2D>();

        PlaceDoorHolder();
    }

    private void PlaceDoorHolder()
    {
        var tilemap = DoorTilemap.ParentTilemapGroup
            .FindTilemapByName("ShipForeground");
        tilemap.SetTile(DoorLocation, DoorHolderTile);
        if (InvertTiles)
            SetFlipForTile(tilemap, DoorLocation);
    }

    private void SetFlipForTile(STETilemap tilemap, Vector2 pos)
    {
        tilemap.SetTileData(pos, tilemap.GetTileData(pos) | Tileset.k_TileFlag_FlipH);
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

        if (InvertTiles)
        {
            SetFlipForTile(DoorTilemap, bot);
            SetFlipForTile(DoorTilemap, top);
        }

        _oldState = state;
        DoorTilemap.UpdateMeshImmediate();
    }
}