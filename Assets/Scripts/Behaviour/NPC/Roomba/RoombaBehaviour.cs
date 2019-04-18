using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using CreativeSpore.SuperTilemapEditor;
using UnityEngine;

namespace Behaviour.NPC.Roomba
{
    public class RoombaBehaviour : MonoBehaviour, ICanOpenDoors
    {
        // TODO: Use the list from SpaceShipEnvironmentTicker
        private readonly List<int> _dirtTiles = new List<int> {64, 96, 128};

        // Capacity of this roomba. If 0, it is full and must be emptied.
        private int _currentCapacity;

        private STETilemap _doodadsBackgroundTilemap;
        private STETilemap _doodadsForegroundTilemap;
        private int _waitingTicks;
        [SerializeField] public int DefaultRoombaCapacity = 10;
        private NavMeshAgent2D navMeshAgent2D;

        private RoombaStationBehaviour TargetStation;

        public TilemapGroup ParentTilemapGroup;


        // Start is called before the first frame update
        private void Start()
        {
            Debug.Assert(ParentTilemapGroup != null);

            _currentCapacity = DefaultRoombaCapacity;

            navMeshAgent2D = GetComponent<NavMeshAgent2D>();
            _doodadsBackgroundTilemap = ParentTilemapGroup.Tilemaps.First(tilemap => tilemap.name == "DoodadsBackground");
            _doodadsForegroundTilemap = ParentTilemapGroup.Tilemaps.First(tilemap => tilemap.name == "DoodadsForeground");
        
            Debug.Assert(_doodadsBackgroundTilemap != null);
            Debug.Assert(_doodadsForegroundTilemap != null);
        }

        // Update is called once per frame
        private void Update()
        {
            if (_waitingTicks > 0)
            {
                _waitingTicks--;
                return;
            }

            if (navMeshAgent2D.hasPath && Math.Abs(navMeshAgent2D.remainingDistance) < 0.2)
            {
                navMeshAgent2D.ResetPath();
            }

            HandleSearchDirt();
            HandleDocking();


            CheckForDirtAndCleanIfPossible();
        }

        private void HandleDocking()
        {
            if (!navMeshAgent2D.hasPath && _currentCapacity <= 0)
            {
                if (TargetStation != null)
                {
                    // I am at the station.
                    TargetStation.StationaryRoomba = this;
                    _waitingTicks = 60 * 4;
                    _currentCapacity = DefaultRoombaCapacity;
                }
                else
                {
                    // Go to the station
                    GoToNextChargingStation();
                }
            }
            else if (TargetStation)
            {
                TargetStation.StationaryRoomba = null;
                TargetStation = null;
            }
        }

        private void HandleSearchDirt()
        {
            if (!navMeshAgent2D.hasPath && _currentCapacity > 0)
            {
                // Find the nearest dirt tile
                _waitingTicks = 60;
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
                    var gridWorldPos = TilemapUtils.GetGridWorldPos(
                        _doodadsBackgroundTilemap,
                        (int) choosenPosition.x,
                        (int) choosenPosition.y);
                    // add 0.25f to y so the roomba is on top of dirt tiles
                    navMeshAgent2D.destination = gridWorldPos + new Vector3(0, 0.25f, 0);
                }
            }
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
                _doodadsBackgroundTilemap.Erase(gridPosition);
                _doodadsBackgroundTilemap.UpdateMesh();
                _currentCapacity--;
            }
        }

        private void GoToNextChargingStation()
        {
            var nextStation = FindNextRoombaStation();
            if (nextStation == null)
            {
                Debug.Log(gameObject.name+": No roomba station found!");
                return;
            }

            navMeshAgent2D.destination = nextStation.transform.position - new Vector3(0.5f, 0);
            Debug.Log(gameObject.name+": Go to station at " + navMeshAgent2D.destination);
            TargetStation = nextStation;
        }


        private bool IsDirtTile(uint tileData)
        {
            return _dirtTiles.Contains(Tileset.GetTileIdFromTileData(tileData));
        }

        private RoombaStationBehaviour FindNextRoombaStation()
        {
            RoombaStationBehaviour choosenStation = null;
            var lowestDist = float.MaxValue;

            TilemapUtils.IterateTilemapWithAction(_doodadsForegroundTilemap, (tilemap, gx, gy, data) =>
            {
                var tile = tilemap.GetTile(gx, gy);
                if (tile?.paramContainer == null) return;
                if (!tile.paramContainer.GetBoolParam("IsRoombaStation")) return;


                var roombaStation = tilemap.GetTileObject(gx, gy).GetComponent<RoombaStationBehaviour>();
                if (!roombaStation.CanRoombaDockHere(this)) return;

                var gridWorldPos = TilemapUtils.GetGridWorldPos(
                    _doodadsForegroundTilemap,
                    gx,
                    gy);
                var transformPosition = transform.position.magnitude - gridWorldPos.magnitude;
                if (transformPosition < lowestDist)
                {
                    System.Console.WriteLine("gridWorldPos " + gridWorldPos);
                    System.Console.WriteLine("roombaStation " + roombaStation.transform);
                    choosenStation = roombaStation;
                    lowestDist = transformPosition;
                }
            });

            return choosenStation;
        }
    }
}