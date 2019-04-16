using System.Collections.Generic;
using System.Linq;
using Behaviour.Player;
using Behaviour.World.Transport;
using CreativeSpore.SuperTilemapEditor;
using DataModel;
using SpaceShip;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace Behaviour
{
    public class GameManager : MonoBehaviour
    {
        public PlayerBehaviour Player;
        public TilemapGroup ShipTilemapGroup;
        public GameObject ConsoleComputer;
        public GameState GameState { get; private set; }

        private List<ITickable> _tickables = new List<ITickable>();
        public Dictionary<string, IStartable> Startables { get; } = new Dictionary<string, IStartable>();

        private void Start()
        {
            GameState = new GameState();

            InitializeSpaceShip();
            InitializeGalaxy();

            RegisterTickables();
            RegisterStartables();

            GameState.Inialized = true;
        }

        private void RegisterStartables()
        {
            Startables.Add("engine", GameState.PlayerData.SpaceShipData.Engine);
        }

        private void RegisterTickables()
        {
            _tickables.Add(new SpaceShipTicker());
            _tickables.Add(new SpaceShipEnvironmentTicker());
        }

        private void FixedUpdate()
        {
            if (!GameState.Inialized) return;

            _tickables.ForEach(tickable => tickable.Tick());
        }

        private void InitializeGalaxy()
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

        private void InitializeSpaceShip()
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

        public List<AbstractTransportInteractible> TransportInteractibles = new List<AbstractTransportInteractible>();

        public void AddTransportInteractible(AbstractTransportInteractible transport)
        {
            TransportInteractibles.Add(transport);
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