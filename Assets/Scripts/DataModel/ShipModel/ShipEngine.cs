using System.Linq;
using System.Text;
using Behaviour;
using DataModel;
using Util;

namespace UnityEngine
{
    public class ShipEngine : IStartable
    {
        public float MaxSpeed { get; } = 1 / 4f * UnitHelper.AU_H_SPEED_OF_LIGHT;
        public float Acceleration { get; } = 1000; //km/s
        public float CurrentSpeed { get; set; } = 0; //au/h
        private bool _isRunning = false;

        public void Start()
        {
            _isRunning = true;
            //AudioEffectManager.Instance.PlayAudio("Engine");
            AudioEffectManager.Instance.PlayAudioFadeIn("Engine", 0.1f, 3);
            DisplayMovingScreen();
        }


        public void Stop()
        {
            _isRunning = false;
            CurrentSpeed = 0;
            //AudioEffectManager.Instance.StopAudio("Engine");
            AudioEffectManager.Instance.PlayAudioFadeOut("Engine", 1);
            
            DisplayStaticScreen();
        }

        public bool IsRunning()
        {
            return _isRunning;
        }
        
        public string ToConsoleString()
        {
            return $"Speed current: {CurrentSpeed:F3} AU/h\n" +
                   $"Speed maximum: {MaxSpeed:F3} AU/h\n" +
                   $"Acceleration: {Acceleration:F3} km/s";
        }
        
        
        private void DisplayStaticScreen()
        {
            //todo remove hack
            var tileMap = GameManager.Instance
                .ShipTilemapGroup
                .Tilemaps
                .First(tilemap => tilemap.name == "DoodadsForeground");
            
            tileMap.SetTile(-7, 12, 256);
            tileMap.SetTile(-6, 12, 257);
            
            tileMap.UpdateMesh();
        }
        
        private void DisplayMovingScreen()
        {
            //todo remove hack
            var tileMap = GameManager.Instance
                .ShipTilemapGroup
                .Tilemaps
                .First(tilemap => tilemap.name == "DoodadsForeground");
            
            tileMap.Erase(-7, 12);
            tileMap.Erase(-6, 12);
            
            tileMap.UpdateMesh();
        }
    }
}