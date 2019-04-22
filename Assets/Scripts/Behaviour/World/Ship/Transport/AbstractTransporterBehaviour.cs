using System;
using UnityEngine;

namespace Behaviour.World.Transport
{
    public abstract class AbstractTransporterBehaviour : MonoBehaviour
    {
        public abstract TansportType TansportType { get; }

        public abstract bool CanTransportEntities { get; }
        public abstract bool CanReceiveEntities { get; }

        public bool TransportEntity(GameObject go, AbstractTransporterBehaviour target)
        {
            Debug.Assert(target.TansportType == TansportType);
            if (!CanTransportEntities) return false;
            if (!target.CanReceiveEntities) return false;

            // The rooms are of equal size, at least at this state of development.
            // So we can just use a position offset and transport the entity.
            var relativeOffset = go.transform.position - transform.position;
            go.transform.position = target.transform.position + relativeOffset;
            return true;
        }
    }

    public enum TansportType
    {
        None,
        Lift,
        Teleporter
    }
}