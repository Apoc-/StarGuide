using Behaviour.Console;
using Behaviour.Player;
using CreativeSpore.SuperTilemapEditor;
using DataModel;
using UnityEngine;

namespace Behaviour
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();
                }
                return _instance;
            }
        }

        public PlayerBehaviour Player;
        public TilemapGroup ShipTilemapGroup;
        public GameObject ConsoleComputer;
        public GameState GameState { get; private set; }

        private void Start()
        {
            GameState = new GameState();
            
            var spaceShip = new SpaceShip();
            spaceShip.Name = "SGS Mijago";
            spaceShip.Position = Vector3Int.zero;
            spaceShip.Engine = new ShipEngine();
            
            var playerData = new PlayerData();
            playerData.SpaceShip = spaceShip;

            GameState.PlayerData = playerData;

            var galaxyData = new GalaxyData();
            galaxyData.Name = "Sefardim";
            
            var ssg = new StarSystemGenerator(galaxyData);
            var starSystem = ssg.GenerateStarSystem(playerData.SpaceShip.Position);

            galaxyData.StarSystems.Add(starSystem.Position, starSystem);
            GameState.GalaxyData = galaxyData;
            
            Debug.Log(GameState.GalaxyData);
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

        public void ToggleConsoleVisibility()
        {
            var state = ConsoleComputer.gameObject.GetComponent<Canvas>().enabled;
            ConsoleComputer.gameObject.GetComponent<Canvas>().enabled = !state;
        }
    }
}