using UnityEngine;

namespace Behaviour
{
    public class PlayerMovementController : MonoBehaviour
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

        void Start()
        {
            _animController = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            var input = new Vector2(
                Input.GetAxis("Horizontal"), 
                Input.GetAxis("Vertical"));

            HandleAnimation(input);
            
            _rb.velocity = new Vector2(
                Mathf.Lerp(0, input.x * Speed, 0.8f),
                Mathf.Lerp(0, input.y * Speed, 0.8f));
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