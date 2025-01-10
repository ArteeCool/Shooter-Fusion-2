using System;
using UnityEngine;

namespace Code.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Moving settings")]
        [SerializeField] private Single _walkSpeed;
        [SerializeField] private Single _runSpeed;
        [SerializeField] private Single _sprintSpeed;

        [SerializeField] private Single _jumpForce;
        [SerializeField, Range(0.0f, 1.0f)] private Single _inertiaForce = 1;
        [SerializeField, Range(0.0f, 1.0f)] private Single _gravityForce = 1;

        [SerializeField] private AnimationCurve _jumpingCurve;

        [Space] 
        [Header("Binds")]
        [SerializeField] private KeyCode _jump;
        [SerializeField] private KeyCode _sprint;
        
        private const Single GlobalGravity = 9.81f;

        private Single _jumpTime; 
        private Single _gravityAcceleration;
        private Single _speed;
        
        private CharacterController _characterController;

        private Vector3 _velocity;

        private Boolean _isJumping;
        


        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            Movement();
            ApplyGravity();
            _characterController.Move(_velocity * Time.fixedDeltaTime);
        }

        private void Update()
        {
            _speed = Input.GetAxis("Vertical") > 0 
                ? (Input.GetKey(_sprint) ? _sprintSpeed : _runSpeed) 
                : _walkSpeed;
            
            if (!_characterController.isGrounded) return;

            _gravityAcceleration = 0.0f;

            if (Input.GetKeyDown(_jump))
            {
                _jumpTime = 0.0f;
                _isJumping = true;
            }
        }

        private void ApplyGravity()
        {
            if (_isJumping)
            {
                if (_jumpTime < _jumpingCurve.keys[_jumpingCurve.length - 1].time)
                {
                    if (_characterController.isGrounded)
                    {
                        _gravityAcceleration = 0.0f;
                        _isJumping = false;
                    }
                    _jumpTime += Time.fixedDeltaTime;
                    float curveValue = _jumpingCurve.Evaluate(_jumpTime);
                    _gravityAcceleration = _jumpForce * curveValue * Time.fixedDeltaTime;
                }
                else
                {
                    _gravityAcceleration = 0.0f;
                    _isJumping = false;
                }
            }
            else
            {
                _gravityAcceleration -= GlobalGravity * _gravityForce * Time.fixedDeltaTime;
            }

            _velocity.y += _gravityAcceleration;
        }

        private void Movement()
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            
            targetVelocity = transform.TransformDirection(targetVelocity.normalized) * _speed;
            _velocity = Vector3.Lerp(_velocity, targetVelocity, _inertiaForce);
        }

        public Single GetRunSpeed()
        {
            return _runSpeed;
        }
    }
}
