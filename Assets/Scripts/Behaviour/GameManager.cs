using System.Collections.Generic;
using System.Linq;
using Behaviour.Player;
using Behaviour.World;
using Behaviour.World.Transport;
using CreativeSpore.SuperTilemapEditor;
using DataModel;
using SpaceShip;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace Behaviour
{
    public class GameManager : MonoBehaviour
    {
        public PlayerBehaviour Player;
        public TilemapGroup ShipTilemapGroup;
        public GameObject ConsoleComputer;
        public GameObject ShipsGameObject;
        public GameState GameState { get; private set; }

        private List<ITickable> _tickables = new List<ITickable>();
        public Dictionary<string, IStartable> Startables { get; } = new Dictionary<string, IStartable>();

        private List<ShipBehaviour> _ships = new List<ShipBehaviour>();
        
        private void Start()
        {
            GameState = new GameState();

            InitializeSpaceShipData();
            InitializeGalaxyData();

            InitializeSpaceShipBehaviours();
            InitializeLiftDialogs();
            
            RegisterTickables();
            RegisterStartables();

            GameState.Inialized = true;
        }

        private void InitializeLiftDialogs()
        {
            _ships.ForEach(ship =>
            {
                var lifts = ship.Decks.SelectMany(pair => pair.Value).SelectMany(deck => deck.Lifts);
                foreach (var liftBehaviour in lifts)
                {
                    if (liftBehaviour.IsSupportLift) continue;
                    
                    liftBehaviour.BuildLiftControlDialog();
                }
            });
        }

        private void InitializeSpaceShipBehaviours()
        {
            _ships = ShipsGameObject.GetComponentsInChildren<ShipBehaviour>().ToList();
            _ships.ForEach(ship => ship.Initialize());
        }

        private void RegisterStartables()
        {
            Startables.Add("engine", GameState.PlayerData.SpaceShipData.Engine);
        }

        private void RegisterTickables()
        {
            _tickables.Add(new SpaceShipTicker());
        }


        private void FixedUpdate()
        {
            if (!GameState.Inialized) return;

            _tickables.ForEach(tickable => tickable.Tick());
        }

        private void InitializeGalaxyData()
        {
            var galaxyData = new GalaxyData();
            galaxyData.Name = "Sefardim";

            var ssg = new StarSystemGenerator(galaxyData);
            var starSystem = ssg.GenerateStarSystem(GameState.PlayerData.SpaceShipData.GridPosition);

            galaxyData.StarSystems.Add(starSystem.Position, starSystem);
            GameState.GalaxyData = galaxyData;
        }

        public void DisableConsole()
        {
            ConsoleComputer.gameObject.SetActive(false);
            Player.InputController.StopWorking();
        }

        public void EnableConsole()
        {
            ConsoleComputer.gameObject.SetActive(true);
        }

        private void InitializeSpaceShipData()
        {
            var spaceShip = new SpaceShipData();
            spaceShip.Name = "SGS Mijago";
            spaceShip.GridPosition = Vector3Int.zero;
            spaceShip.Position = spaceShip.GridPosition;
            spaceShip.Engine = new ShipEngine();
            
            var playerData = new PlayerData();
            playerData.SpaceShipData = spaceShip;

            GameState.PlayerData = playerData;
        }

        #region singleton

        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        #endregion
    }
}