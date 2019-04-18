using UnityEngine;

namespace Behaviour.NPC.Roomba
{
    public class RoombaStationBehaviour : MonoBehaviour
    {
        public RoombaBehaviour StationaryRoomba = null;

        private bool IsEmpty => StationaryRoomba == null;


        public bool CanRoombaDockHere(RoombaBehaviour rb)
        {
            return IsEmpty;
        }
    }
}