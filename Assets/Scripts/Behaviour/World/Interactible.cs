using UnityEngine;

namespace Behaviour.World
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class Interactible : MonoBehaviour
    {
        public abstract void OnInteract();
    }
}