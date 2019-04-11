using System.Collections;
using System.Collections.Generic;
using Behaviour;
using UnityEngine;

public class RoombaStationBehaviour : MonoBehaviour
{
    public RoombaBehaviour StationaryRoomba = null;

    private bool IsEmpty => StationaryRoomba == null;


    public bool CanRoombaDockHere(RoombaBehaviour rb)
    {
        return IsEmpty;
    }
}