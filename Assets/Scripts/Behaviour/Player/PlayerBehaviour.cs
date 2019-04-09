using DataModel;
using UnityEngine;

namespace Behaviour.Player
{
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayerBehaviour : MonoBehaviour
    {
        public PlayerInputController InputController { get; private set; }

        private void Start()
        {
            InputController = GetComponent<PlayerInputController>();
        }
    }
}