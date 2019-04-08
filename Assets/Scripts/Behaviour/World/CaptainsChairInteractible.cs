using UnityEngine;

namespace Behaviour.World
{
    public class CaptainsChairInteractible : Interactible
    {
        public override void OnInteract(PlayerInputController playerInputController)
        {
            playerInputController.ToggleSitState();
        }
    }
}