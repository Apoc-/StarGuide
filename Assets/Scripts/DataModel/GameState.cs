using System.Text;

namespace DataModel
{
    public class GameState
    {
        public PlayerData PlayerData;
        public GalaxyData GalaxyData;
        public bool Inialized { get; set; } = false;
    }
}