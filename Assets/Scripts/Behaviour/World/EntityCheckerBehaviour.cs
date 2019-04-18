using System.Collections.Generic;
using Behaviour.NPC;
using UnityEngine;

namespace Behaviour.World
{
    public class EntityCheckerBehaviour : MonoBehaviour
    {
        public readonly HashSet<EntityBehaviour> ContainedEntities = new HashSet<EntityBehaviour>();

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var entity = collider.gameObject.GetComponent<EntityBehaviour>();
            if (entity != null)
            {
                ContainedEntities.Add(entity);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            var entity = collider.gameObject.GetComponent<EntityBehaviour>();
            if (entity != null)
            {
                ContainedEntities.Remove(entity);
            }
        }
    }
}