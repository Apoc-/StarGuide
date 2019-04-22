using Assets.Scripts;
using Behaviour.NPC;
using DataModel;
using UnityEngine;

namespace Behaviour.Player
{
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayerBehaviour : EntityBehaviour, ICanOpenDoors
    {
        private PlayerInputController _inputController;

        public PlayerInputController InputController
        {
            get
            {
                if (_inputController == null)
                {
                    _inputController = GetComponent<PlayerInputController>();
                }

                return _inputController;

            }
        }
    }
}