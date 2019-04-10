using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Behaviour;
using CreativeSpore.SuperTilemapEditor;
using UnityEngine;
using Util;

public class RoombaBehaviour : MonoBehaviour, ICanOpenDoors
{
    // TODO: Use the list from SpaceShipEnvironmentTicker
    private readonly List<int> _dirtTiles = new List<int> {64, 96, 128};
    private STETilemap _doodadsBackgroundTilemap;
    private NavMeshAgent2D navMeshAgent2D;

    // Start is called before the first frame update
    private void Start()
    {
        navMeshAgent2D = GetComponent<NavMeshAgent2D>();
        _doodadsBackgroundTilemap = GameManager.Instance
            .ShipTilemapGroup
            .Tilemaps
            .First(tilemap => tilemap.name == "DoodadsBackground");
    }

    // Update is called once per frame
    private void Update()
    {
        // DEBUG: SET POSITION VIA CLICK
        if (Input.GetMouseButton(0))
        {
            var w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            navMeshAgent2D.destination = w;
        }


        // Find the nearest dirt tile
        if (Math.Abs(navMeshAgent2D.remainingDistance) < 0.01)
        {
            Vector2 choosenPosition = Vector2.positiveInfinity;
            float lowestDist = Single.MaxValue;

            TilemapUtils.IterateTilemapWithAction(_doodadsBackgroundTilemap, (tilemap, gx, gy, data) =>
            {
                if (IsDirtTile(data))
                {
                    var gridWorldPos = TilemapUtils.GetGridWorldPos(
                        _doodadsBackgroundTilemap,
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
            if (Math.Abs(lowestDist - Single.MaxValue) > 0.01)
            {
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
            _doodadsBackgroundTilemap.Erase(gridPosition);
            _doodadsBackgroundTilemap.UpdateMesh();
        }
    }

    private bool IsDirtTile(uint tileData)
    {
        return _dirtTiles.Contains(Tileset.GetTileIdFromTileData(tileData));
    }
}