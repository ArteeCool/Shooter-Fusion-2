using System;
using UnityEngine;

namespace Code.Controllers
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _playerTransform;
        
        [Header("Camera Settings")]
        [SerializeField] private Vector3 _offset;
        [SerializeField, Range(0.0f, 5.0f)] private Single _sensitivity = 1.0f;

        [Header("Pitch Limits")]
        [SerializeField] private Int32 _maxXRotation;
        [SerializeField] private Int32 _minXRotation;


        private Single _pitch;
        private Single _yaw;

        private Boolean _isFocused;

        private HeadBob _headBob;

        private void Awake()
        {
            _headBob = GetComponent<HeadBob>();
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Focus();
            }

            if (!_isFocused) return;
            UpdateCameraRotation();
            
            _headBob.ApplyBob();
        }

        private void Focus()
        {
            _isFocused = !_isFocused;
            Cursor.visible = !_isFocused;
            Cursor.lockState = _isFocused ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void UpdateCameraRotation()
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            _pitch -= mouseDelta.y * _sensitivity;
            _yaw += mouseDelta.x * _sensitivity;

            _pitch = Mathf.Clamp(_pitch, _minXRotation, _maxXRotation);

            _playerTransform.localEulerAngles = new Vector3(0.0f, _yaw, 0.0f);
            _cameraTransform.localEulerAngles = new Vector3(_pitch, 0.0f, 0.0f);
        }
    }
}
