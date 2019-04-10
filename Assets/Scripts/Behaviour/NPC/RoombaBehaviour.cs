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
            var tiles = new List<Vector2>();
            TilemapUtils.IterateTilemapWithAction(_doodadsBackgroundTilemap, (tilemap, gx, gy, data) =>
            {
                if (IsDirtTile(data))
                    tiles.Add(new Vector2(gx, gy));
            });

            if (tiles.Count != 0)
            {
                var toSkip = MathHelper.GetRandomInt(0, tiles.Count);
                var randPos = tiles.Skip(toSkip).Take(1).First();
                var gridWorldPos = TilemapUtils.GetGridWorldPos(
                    _doodadsBackgroundTilemap,
                    (int) randPos.x,
                    (int) randPos.y);
                navMeshAgent2D.destination = gridWorldPos;
            }

            /*
            var oldDest = navMeshAgent2D.destination;

            Vector2 nearest = new Vector2();
            float lowestDistance = Single.MaxValue;
            TilemapUtils.IterateTilemapWithAction(_doodadsBackgroundTilemap, (tilemap, x, y, tileData) =>
            {
                // I only want dirt tiles
                if (!IsDirtTile(tileData)) return;
                
                
                var worldPosition = TilemapUtils.GetGridWorldPos(x, y, _doodadsBackgroundTilemap.CellSize);

                navMeshAgent2D.destination = worldPosition;
                if (!navMeshAgent2D.hasPath) return;

                if (navMeshAgent2D.remainingDistance < lowestDistance)
                {
                    lowestDistance = navMeshAgent2D.remainingDistance;
                    nearest = worldPosition;
                }
            });
            if (Math.Abs(lowestDistance - Single.MaxValue) > 0.01)
            {
                navMeshAgent2D.destination = nearest;
            }
            else
            {
                navMeshAgent2D.destination = oldDest;
            }
        */
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