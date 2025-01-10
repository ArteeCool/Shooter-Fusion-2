using System;
using UnityEngine;

namespace Code.Controllers
{
    public class HeadBob : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _cameraPivot;
        [SerializeField] private Transform _handsPivot;

        [Header("Bob Settings")] 
        [SerializeField] private Boolean _enable;
        [Space]
        [SerializeField, Range(0.0f, 0.1f)] private Single _cameraAmplitude = 0.015f;
        [SerializeField, Range(0.0f, 30.0f)] private Single _cameraFrequency = 0.015f; 
        [SerializeField, Range(0.0f, 0.1f)] private Single _handsAmplitude = 0.015f;
        [SerializeField, Range(0.0f, 30.0f)] private Single _handsFrequency = 0.015f;
        [SerializeField] private Single _cameraSmooth;
        [SerializeField] private Single _handsSmooth;
        [SerializeField] private Single _jumpBob;

        private Vector3 _cameraStartPos;
        private Vector3 _handsStartPos;
        
        private CharacterController _characterController;
        
        private Single _bobToggleSpeed;

        private Single _shiftMultiplier = 1.0f;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _cameraStartPos = _cameraPivot.localPosition;
            _handsStartPos = _handsPivot.localPosition;
            _bobToggleSpeed = GetComponent<PlayerMovementController>().GetRunSpeed();
        }

        public void ApplyBob()
        {
            if (!_enable) return;

            _shiftMultiplier = Input.GetKey(KeyCode.LeftShift) ? 2.0f : 1.0f;
            
            CheckForBob();
            ResetBob();
        }

        private void CheckForBob()
        {
            Vector3 speed = _characterController.velocity;
            speed.y = 0;
            
            if (!_characterController.isGrounded) return;
            if (Mathf.Ceil(speed.magnitude) < _bobToggleSpeed) return;
            
            CameraBob();
            HandsBob();
        }

        private void ResetBob()
        {
            if (_cameraPivot.localPosition == _cameraStartPos) return;
            _cameraPivot.localPosition = Vector3.Lerp(_cameraPivot.localPosition, _cameraStartPos, 1.0f * Time.deltaTime);            
            
            if (_handsPivot.localPosition == _handsStartPos) return;
            _handsPivot.localPosition = Vector3.Lerp(_handsPivot.localPosition, _handsStartPos, 1.0f * Time.deltaTime);            
        }        

        private void CameraBob()
        {
            Vector3 pos = Vector3.zero;

            pos.y = Mathf.Lerp(pos.y, Mathf.Sin(Time.time * _cameraFrequency) * _cameraAmplitude * 1.4f,
                _cameraSmooth * Time.deltaTime);            
            pos.x = Mathf.Lerp(pos.x, Mathf.Sin(Time.time * _cameraFrequency / 2.0f) * _cameraAmplitude * 1.6f,
                _cameraSmooth * Time.deltaTime);

            _cameraPivot.transform.localPosition += pos * _shiftMultiplier;
        }        
        
        private void HandsBob()
        {
            Vector3 pos = Vector3.zero;

            pos.y = Mathf.Lerp(pos.x, Mathf.Sin(Time.time * _handsFrequency) * _handsAmplitude * 1.4f,
                _handsSmooth * Time.deltaTime);            
            pos.x = -Mathf.Lerp(pos.y, Mathf.Sin(Time.time * _handsFrequency / 2.0f) * _handsAmplitude * 1.6f,
                _handsSmooth * Time.deltaTime);

            _handsPivot.transform.localPosition += pos * _shiftMultiplier;
        }
    }
}
