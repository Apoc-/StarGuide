using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Behaviour;
using CreativeSpore.SuperTilemapEditor;
using UnityEngine;

public class RoombaBehaviour : MonoBehaviour, ICanOpenDoors
{
    // TODO: Use the list from SpaceShipEnvironmentTicker
    private readonly List<int> _dirtTiles = new List<int> {64, 96, 128};

    // Capacity of this roomba. If 0, it is full and must be emptied.
    private int _currentCapacity;

    private STETilemap _doodadsBackgroundTilemap;
    private STETilemap _doodadsForegroundTilemap;
    [SerializeField] public int DefaultRoombaCapacity = 10;
    private NavMeshAgent2D navMeshAgent2D;


    // Start is called before the first frame update
    private void Start()
    {
        _currentCapacity = DefaultRoombaCapacity;

        navMeshAgent2D = GetComponent<NavMeshAgent2D>();
        _doodadsBackgroundTilemap = GameManager.Instance
            .ShipTilemapGroup
            .Tilemaps
            .First(tilemap => tilemap.name == "DoodadsBackground");

        _doodadsForegroundTilemap = GameManager.Instance
            .ShipTilemapGroup
            .Tilemaps
            .First(tilemap => tilemap.name == "DoodadsForeground");
    }

    // Update is called once per frame
    private void Update()
    {
        // Find the nearest dirt tile
        if (Math.Abs(navMeshAgent2D.remainingDistance) < 0.01 && _currentCapacity == 0)
        {
            Debug.Log("Search new Station");
            GoToNextChargingStation();
            _currentCapacity = DefaultRoombaCapacity;
        }
        else if (Math.Abs(navMeshAgent2D.remainingDistance) < 0.1 && _currentCapacity > 0)
        {
            Debug.Log("Search new Dirt");
            var choosenPosition = Vector2.positiveInfinity;
            var lowestDist = float.MaxValue;

            TilemapUtils.IterateTilemapWithAction(_doodadsBackgroundTilemap, (tilemap, gx, gy, data) =>
            {
                if (IsDirtTile(data))
                {
                    var gridWorldPos = TilemapUtils.GetGridWorldPos(
                        _doodadsBackgroundTilemap,
                        gx,
                        gy);
                    var transformPosition = Vector2.Distance(transform.position, gridWorldPos);
                    if (transformPosition < lowestDist)
                    {
                        lowestDist = transformPosition;
                        choosenPosition = new Vector2(gx, gy);
                    }
                }
            });

            if (Math.Abs(lowestDist - float.MaxValue) > 0.01)
            {
                Debug.Log("Go to " + choosenPosition + " with dist " + lowestDist);
                var gridWorldPos = TilemapUtils.GetGridWorldPos(
                    _doodadsBackgroundTilemap,
                    (int) choosenPosition.x,
                    (int) choosenPosition.y);
                navMeshAgent2D.destination = gridWorldPos;
            }
        }


        CheckForDirtAndCleanIfPossible();
    }

    private void CheckForDirtAndCleanIfPossible()
    {
        if (_currentCapacity == 0)
            return;

        // Check tile below me
        var realWorldPosition = transform.position;

        var vLocPos = _doodadsBackgroundTilemap.transform.InverseTransformPoint(realWorldPosition);
        var gridX = BrushUtil.GetGridX(vLocPos, _doodadsBackgroundTilemap.CellSize);
        var gridY = BrushUtil.GetGridY(vLocPos, _doodadsBackgroundTilemap.CellSize);


        var gridPosition = TilemapUtils.GetGridPosition(
            _doodadsBackgroundTilemap,
            new Vector2(realWorldPosition.x, realWorldPosition.y)
        );

        gridPosition = new Vector2(gridX, gridY);

        var tileData = _doodadsBackgroundTilemap.GetTileData(gridPosition);

        // Remove the tile below me!
        if (IsDirtTile(tileData))
        {
            Debug.Log("Got Dirt!");
            _doodadsBackgroundTilemap.Erase(gridPosition);
            _doodadsBackgroundTilemap.UpdateMesh();
            _currentCapacity--;
        }
    }

    private void GoToNextChargingStation()
    {
        var nextStation = FindNextRoombaStation();
        if (!nextStation.HasValue)
        {
            Debug.Log("No roomba station found!");
            return;
        }

        var gridWorldPos = _doodadsBackgroundTilemap.transform.TransformPoint(
            new Vector2(
                (nextStation.Value.x + .5f) * _doodadsBackgroundTilemap.CellSize.x,
                (nextStation.Value.y + .5f) * _doodadsBackgroundTilemap.CellSize.y
            )
        );
        navMeshAgent2D.destination = gridWorldPos;
    }


    private bool IsDirtTile(uint tileData)
    {
        return _dirtTiles.Contains(Tileset.GetTileIdFromTileData(tileData));
    }

    private Vector2? FindNextRoombaStation()
    {
        var choosenPosition = Vector2.positiveInfinity;
        var lowestDist = float.MaxValue;

        TilemapUtils.IterateTilemapWithAction(_doodadsForegroundTilemap, (tilemap, gx, gy, data) =>
        {
            var tile = tilemap.GetTile(gx, gy);
            if (tile?.paramContainer == null) return;
            if (tile.paramContainer.GetBoolParam("IsRoombaStation"))
            {
                var gridWorldPos = TilemapUtils.GetGridWorldPos(
                    _doodadsForegroundTilemap,
                    gx,
                    gy);
                var transformPosition = transform.position.magnitude - gridWorldPos.magnitude;
                if (transformPosition < lowestDist)
                {
                    lowestDist = transformPosition;
                    choosenPosition = new Vector2(gx, gy);
                }
            }
        });

        if (Math.Abs(lowestDist - float.MaxValue) > 0.01)
            return choosenPosition;

        return null;
    }
}