using System.Linq;
using UnityEngine;

namespace Behaviour.World
{
    public class MainComputerInteractible: WorkInteractible
    {        
        public override void OnInteract(PlayerInputController playerInputController)
        {
            GameManager.Instance.EnableConsole();
            
            
            base.OnInteract(playerInputController);
        }
    }
}