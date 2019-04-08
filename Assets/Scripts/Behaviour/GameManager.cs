using Behaviour.Player;
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

        private GameState _gameState;
        
        private void Start()
        {
            _gameState = new GameState();
            
            var spaceShip = new SpaceShip();
            spaceShip.Name = "SGS Mijago";
            spaceShip.Position = Vector3Int.zero;
            
            var playerData = new PlayerData();
            playerData.SpaceShip = spaceShip;

            _gameState.PlayerData = playerData;

            var galaxyData = new GalaxyData();
            galaxyData.Name = "Sefardim";
            
            var ssg = new StarSystemGenerator(galaxyData);
            var starSystem = ssg.GenerateStarSystem(playerData.SpaceShip.Position);

            galaxyData.StarSystems.Add(starSystem.Position, starSystem);
            _gameState.GalaxyData = galaxyData;
            
            Debug.Log(_gameState.GalaxyData);


            /*Debug.Log("----------");
            var pg = new PlanetGenerator(starSystem);
            for (int i = 0; i < 10000; i++)
            {
                var p = pg.GeneratePlanet(PlanetSize.Small);

                Debug.Log(p.Position);
            }*/
        }
    }
}