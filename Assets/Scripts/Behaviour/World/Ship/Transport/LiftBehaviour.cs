using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Behaviour.World.Door;
using Behaviour.World.Transport;
using CreativeSpore.RPGConversationEditor;
using UnityEngine;
using UnityEngine.Events;
using Util;
using Util.ConversationBuilder;

namespace Behaviour.World
{
    public class LiftBehaviour : AbstractTransporterBehaviour, IInitializable
    {
        [Header("A list of all doors leading to this lift.")]
        public List<DoorBehaviour> Doors;

        [Header("Only check this if this is the lift used to go from A to B")]
        public bool IsSupportLift;

        public LiftBehaviour SupportLiftReference;

        public ConversationController ConversationController;

        public override TansportType TansportType => TansportType.Lift;
        public override bool CanTransportEntities => IsSupportLift || !Doors.Any(door => door.IsOpen);
        public override bool CanReceiveEntities => IsSupportLift || !Doors.Any(door => door.IsOpen);

        private DeckBehaviour _deck;

        public bool IsDriving
        {
            get => _windowAnimator.GetBool("IsDriving");
            set => _windowAnimator.SetBool("IsDriving", value);
        }

        private Animator _windowAnimator;

        private LiftBehaviour _target = null;
        private EntityCheckerBehaviour _entityChecker;

        public void Start()
        {
            
            _windowAnimator = GetComponentInChildren<Animator>();
            _entityChecker = GetComponentInChildren<EntityCheckerBehaviour>();
        }

        public void DriveToTarget(LiftBehaviour target)
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
            _target.IsDriving = false;
            SupportLiftReference.IsDriving = false;
            
            yield return new WaitForSeconds(0.6f);

            SupportLiftReference.MoveAllEntities(_target);
            _target = null;
        }

        private void MoveAllEntities(LiftBehaviour target)
        {
            foreach (var entityCheckerContainedEntity in _entityChecker.ContainedEntities)
            {
                // The rooms are of equal size, at least at this state of development.
                // So we can just use a position offset and transport the entity.
                var containedEntities = entityCheckerContainedEntity.transform;
                var relativeOffset = containedEntities.position - transform.position;
                containedEntities.position = relativeOffset + target.transform.position;
            }
        }

        // for debugging purposes, teleport to a random place
        /*public override void OnInteract(PlayerInputController playerInputController)
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
        }*/
        
        public void BuildLiftControlDialog()
        {
            var conversationBuilder = ConversationBuilder.Create().SetConversationName("Lift Menu");
            var deckData = _deck.Ship.Decks;
            var deckCategories = deckData.Keys;
            var categorySelectDialogBuilder = DialogBuilder
                .Create()
                .SetFreezesPlayerMovement()
                .SetSentence(0,"Select destination category:");
            
            foreach (var deckCategory in deckCategories)
            {
                var ddb = DialogBuilder.Create()
                    .SetFreezesPlayerMovement()
                    .SetSentence(0, $"Category: {deckCategory}");
                
                foreach (var deck in deckData[deckCategory])
                {
                    for (var i = 0; i < deck.Lifts.Count; i++)
                    {
                        var lift = deck.Lifts[i];
                        
                        ddb.AddAction(DialogActionBuilder
                            .Create()
                            .SetText($"{deck.DeckName} - Lift {i+1}")
                            .SetOnSubmitEvent(() =>
                            {
                                DriveToTarget(lift);
                            })
                            .End());
                    }
                }

                var dia = ddb.End();
                conversationBuilder.AddDialogue(dia);
                
                categorySelectDialogBuilder
                    .AddAction(DialogActionBuilder
                        .Create()
                        .SetText($"{deckCategory}")
                        .SetTargetDialog(dia)
                        .End());
            }

            conversationBuilder.AddDialogueAsStartDialog(categorySelectDialogBuilder.End());

            ConversationController.conversations[0] = conversationBuilder.End();
        }

        public void Initialize()
        {
            Debug.Assert(IsSupportLift || Doors != null && Doors.Count > 0, "Must have at least one door!");
            Debug.Assert(IsSupportLift || Doors.Count(door => door == null) == 0, "Door references must not be zero.");
            Debug.Assert(IsSupportLift || SupportLiftReference != null,
                "Lifts must have a reference to the support lift.");

            if (!IsSupportLift)
            {
                _deck = GetComponentInParent<DeckBehaviour>();
            }
           
        }
    }
}