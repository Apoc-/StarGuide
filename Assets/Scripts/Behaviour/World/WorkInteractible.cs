using System.Linq;
using UnityEngine;

namespace Behaviour.World
{
    public class WorkInteractible: Interactible
    {        
        public override void OnInteract(PlayerInputController playerInputController)
        {
            playerInputController.StartWorking(this);
        }
    }
}