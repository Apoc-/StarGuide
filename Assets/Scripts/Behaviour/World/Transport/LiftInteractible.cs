using System.Collections.Generic;
using System.Linq;
using Behaviour.World.Transport;
using UnityEngine;

namespace Behaviour.World
{
    public class LiftInteractible : AbstractTransportInteractible
    {
      

        [Header("The relative tile position of the edges of this room.")]
        public Vector2 TopLeftTile;
        public Vector2 BottomRightTile;
        [Header("A list of all doors leading to this lift.")]
        public List<DoorBehaviour> Doors;

        public override TansportType TansportType => TansportType.Lift;
        public override bool CanTransportEntities => !Doors.Any(door => door.IsOpen);
        public override bool CanReceiveEntities => !Doors.Any(door => door.IsOpen);

        public new void Start()
        {
            base.Start();

            Debug.Assert(Doors != null && Doors.Count > 0, "Must have at least one door!");
//            Debug.Assert(Doors.Any(door => door == null), "Door references must not be zero.");
            
            
            GameManager.Instance.AddTransportInteractible(this);
        }

        // for debugging purposes, teleport to a random place
        public override void OnInteract(PlayerInputController playerInputController)
        {
            Debug.Log("Interacted");

            var target = GameManager.Instance.TransportInteractibles
                .Where(intractable => intractable.TansportType == TansportType)
                .FirstOrDefault(intractable => intractable != this);

            Debug.Log(target);
            if (target != null)
                TransportEntity(playerInputController.gameObject, target);
        }
    }
}