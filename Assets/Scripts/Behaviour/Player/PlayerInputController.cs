using Behaviour.World;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Behaviour
{
    public class PlayerInputController : MonoBehaviour
    {
        public float Speed = 1;

        private Rigidbody2D _rb;
        private Animator _animController;
        private static readonly int Left = Animator.StringToHash("Left");
        private static readonly int Right = Animator.StringToHash("Right");
        private static readonly int Down = Animator.StringToHash("Down");
        private static readonly int Up = Animator.StringToHash("Up");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Sit = Animator.StringToHash("Sit");
        
        private bool _isSitting = false;
        
        void Start()
        {
            _animController = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!_isSitting)
            {
                HandleMovementInput();    
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if(_isSitting) ToggleSitState();
            }

            if (Input.GetButtonDown("Fire1"))
            {
                HandleActivationInput();
            }
        }

        public void ToggleSitState()
        {
            _isSitting = !_isSitting;
            
            _animController.SetBool(Sit, _isSitting);
        }

        private void HandleMovementInput()
        {
            var input = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));

            HandleAnimation(input);

            _rb.velocity = new Vector2(
                Mathf.Lerp(0, input.x * Speed, 0.8f),
                Mathf.Lerp(0, input.y * Speed, 0.8f));
        }

        private void HandleActivationInput()
        {
            var direction = GetCurrentLookingDirection();
            var layerMask = LayerMask.GetMask("Interactibles");

            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, layerMask);

                if (hit.collider != null)
                {
                    var ia = hit.collider.gameObject.GetComponent<Interactible>();
                    if (ia != null)
                    {
                        ia.OnInteract(this);
                    }
                }
            }
        }


        private bool HasStateName(AnimatorStateInfo stateInfo, string name)
        {
            
            return stateInfo.shortNameHash == Animator.StringToHash(name);
        }

        private bool HasAnyStateName(AnimatorStateInfo stateInfo, params string[] names)
        {
            for (var i = 0; i < names.Length; i++)
            {
                if (HasStateName(stateInfo, names[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private Vector2 GetCurrentLookingDirection()
        {
            var state = _animController.GetCurrentAnimatorStateInfo(0);

            if (HasAnyStateName(state, "FlashyLeft", "FlashyStandLeft"))
            {
                return Vector2.left;
            }

            if (HasAnyStateName(state, "FlashyRight", "FlashyStandRight"))
            {
                return Vector2.right;
            }

            if (HasAnyStateName(state, "FlashyUp", "FlashyStandUp"))
            {
                return Vector2.up;
            }

            if (HasAnyStateName(state, "FlashyDown", "FlashyStandDown"))
            {
                return Vector2.down;
            }

            return Vector2.down;
        }

        private void HandleAnimation(Vector2 input)
        {
            _animController.SetFloat(Horizontal, input.x);
            _animController.SetFloat(Vertical, input.y);

            if (input.x > 0)
            {
                _animController.SetBool(Right, true);
            }
            else
            {
                _animController.SetBool(Right, false);
            }

            if (input.y < 0)
            {
                _animController.SetBool(Down, true);
            }
            else
            {
                _animController.SetBool(Down, false);
            }

            if (input.y > 0)
            {
                _animController.SetBool(Up, true);
            }
            else
            {
                _animController.SetBool(Up, false);
            }

            if (input.x < 0)
            {
                _animController.SetBool(Left, true);
            }
            else
            {
                _animController.SetBool(Left, false);
            }
        }
    }
}