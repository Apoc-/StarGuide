using System.Collections.Generic;
using Behaviour.NPC;
using UnityEngine;

namespace Behaviour.World
{
    public class EntityCheckerBehaviour : MonoBehaviour
    {
        public readonly HashSet<GameObject> ContainedEntities = new HashSet<GameObject>();

        private void OnTriggerEnter2D(Collider2D e)
        {
            var component = e.gameObject.GetComponent<IEntity>();
            if (component != null)
            {
                ContainedEntities.Add(e.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D e)
        {
            var component = e.gameObject.GetComponent<IEntity>();
            if (component != null)
            {
                ContainedEntities.Remove(e.gameObject);
            }
        }
    }
}