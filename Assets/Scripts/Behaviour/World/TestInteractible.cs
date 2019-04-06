using UnityEngine;

namespace Behaviour.World
{
    public class TestInteractible : Interactible
    {
        public override void OnInteract()
        {
            Debug.Log("Interacted");
        }
    }
}