using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.World.Door;
using Behaviour.World.Transport;
using UnityEngine;

namespace Behaviour.World
{
    public class LiftInteractible : AbstractTransportInteractible
    {
        [Header("A list of all doors leading to this lift.")]
        public List<DoorBehaviour> Doors;

        [Header("Only check this if this is the lift used to go from A to B")]
        public bool IsSupportLift;

        public LiftInteractible SupportLiftReference;

        public override TansportType TansportType => TansportType.Lift;
        public override bool CanTransportEntities => IsSupportLift || !Doors.Any(door => door.IsOpen);
        public override bool CanReceiveEntities => IsSupportLift || !Doors.Any(door => door.IsOpen);

        public bool IsDriving
        {
            get => _windowAnimator.GetBool("IsDriving");
            set => _windowAnimator.SetBool("IsDriving", value);
        }

        private Animator _windowAnimator;

        private LiftInteractible _target = null;
        private EntityCheckerBehaviour _entityChecker;

        public new void Start()
        {
            base.Start();

            Debug.Assert(IsSupportLift || Doors != null && Doors.Count > 0, "Must have at least one door!");
            Debug.Assert(IsSupportLift || Doors.Count(door => door == null) == 0, "Door references must not be zero.");
            Debug.Assert(IsSupportLift || SupportLiftReference != null,
                "Lifts must have a reference to the support lift.");

            if (!IsSupportLift)
                GameManager.Instance.AddTransportInteractible(this);

            _windowAnimator = GetComponentInChildren<Animator>();
            _entityChecker = GetComponentInChildren<EntityCheckerBehaviour>();
        }

        public void DriveToTarget(LiftInteractible target)
        {
            Debug.Assert(_target == null);
            if (_target != null)
                return;

            // TODO: check if target is occupied or sth
            if (!CanTransportEntities) return;

            _target = target;
            MoveAllEntities(SupportLiftReference);
            
            StartCoroutine(FinishTransportStartCoroutine());
        }
        
        private IEnumerator FinishTransportStartCoroutine()
        {
            _target.IsDriving = true;
            SupportLiftReference.IsDriving = true;
            yield return new WaitForSeconds(3f);
            SupportLiftReference.IsDriving = false;
            _target.IsDriving = false;
            
            yield return new WaitForSeconds(0.6f);

            SupportLiftReference.MoveAllEntities(_target);
            _target = null;
        }

        private void MoveAllEntities(LiftInteractible target)
        {
            foreach (var entityCheckerContainedEntity in _entityChecker.ContainedEntities)
            {
                // The rooms are of equal size, at least at this state of development.
                // So we can just use a position offset and transport the entity.
                var relativeOffset = entityCheckerContainedEntity.transform.position - transform.position;
                entityCheckerContainedEntity.transform.position = relativeOffset + target.transform.position;
            }
        }

        // for debugging purposes, teleport to a random place
        public override void OnInteract(PlayerInputController playerInputController)
        {
            if (IsSupportLift) return;
            Debug.Log("Interacted with Lift");

            
            // TODO: Redo the target selection once the Gui is done in SG-19
            var target = GameManager.Instance.TransportInteractibles
                .OfType<LiftInteractible>()
                .Where(intractable => !intractable.IsSupportLift)
                .FirstOrDefault(intractable => intractable != this);

            if (target != null)
                DriveToTarget(target);
            // TransportEntity(playerInputController.gameObject, target);
        }
    }
}