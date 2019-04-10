using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Behaviour;
using CreativeSpore.SuperTilemapEditor;
using UnityEngine;
using Util;

namespace SpaceShip
{
    public class SpaceShipEnvironmentTicker : ITickable
    {
        private readonly STETilemap _shipBackgroundTilemap;

        private List<Vector2> _dirtiableTiles = new List<Vector2>();
        private readonly List<int> _dirtTiles = new List<int> {224, 225, 226, 227, 228, 229};
        private readonly STETilemap _doodadsBackgroundTilemap;

        public SpaceShipEnvironmentTicker()
        {
            _doodadsBackgroundTilemap = GameManager.Instance
                .ShipTilemapGroup
                .Tilemaps
                .First(tilemap => tilemap.name == "DoodadsBackground");

            _shipBackgroundTilemap = GameManager.Instance
                .ShipTilemapGroup
                .Tilemaps
                .First(tilemap => tilemap.name == "ShipBackground");

            SetDirtiableTiles();
        }

        public void Tick()
        {
            TryToPlaceDirt();
        }

        private void SetDirtiableTiles()
        {
            var data = new List<Vector2>();

            TilemapUtils.IterateTilemapWithAction(_shipBackgroundTilemap, (tilemap, x, y, tileData) =>
            {
                var tile = tilemap.GetTile(x, y);
                if (tile?.paramContainer == null) return;
                if (tile.paramContainer.GetBoolParam("CanBeDirty"))
                    data.Add(new Vector2(x, y));
            });
            _dirtiableTiles = data;
        }


        private int GetRandomDirtTile()
        {
            var toSkip = MathHelper.GetRandomInt(0, _dirtTiles.Count);
            return _dirtTiles.Skip(toSkip).Take(1).First();
        }

        private Vector2? GetRandomPlaceableTilePosition()
        {
            var dirtyCount = 0;
            var freePositions = new List<Vector2>();
            foreach (var tileVector in _dirtiableTiles)
            {
                var tileData = _doodadsBackgroundTilemap.GetTileData(tileVector);
                if (Tileset.k_TileData_Empty == tileData)
                {
                    freePositions.Add(tileVector);
                } else if (IsDirtTile(tileData))
                {
                    dirtyCount++;
                }
            }
            
            if (freePositions.Count == 0 || dirtyCount >= 5)
                return null;

            var toSkip = MathHelper.GetRandomInt(0, freePositions.Count);
            var randomTile = freePositions.Skip(toSkip).Take(1).First();

            return randomTile;
        }

        private bool IsDirtTile(uint tileData)
        {
            return _dirtTiles.Contains(Tileset.GetTileIdFromTileData(tileData));
        }

        private void TryToPlaceDirt()
        {
            var randomTile = GetRandomPlaceableTilePosition();
            if (randomTile.HasValue)
            {
                Debug.Log("Place Dirt at "+randomTile.Value);
                _doodadsBackgroundTilemap.SetTile(randomTile.Value, GetRandomDirtTile());
            }
        }
    }
}