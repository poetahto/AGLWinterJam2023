using poetools.Abstraction;
using UnityEngine;

namespace poetools.NewDirectory1
{
    // todo: clamp rotation
    public class FPSRotationSystem : MonoBehaviour
    {
        public float sensitivity = 1;
        public bool invertY;

        public TransformComponent PitchRotation;
        public TransformComponent YawRotation;

        private float _currentPitch;
        private float _currentYaw;
        
        private void OnEnable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void ApplyDelta(Vector2 mouseDelta)
        {
            float yawAngle = mouseDelta.x * sensitivity;
            float pitchAngle = mouseDelta.y * sensitivity * (invertY ? 1 : -1);

            _currentYaw = Mathf.Repeat(_currentYaw + yawAngle, 360);
            _currentPitch = Mathf.Clamp(_currentPitch + pitchAngle, -90, 90);
        
            PitchRotation.Rotation = Quaternion.Euler(_currentPitch, 0, 0);
            YawRotation.Rotation = Quaternion.Euler(0, _currentYaw, 0);
        }

        public void SetRotation(Vector3 euler)
        {
            _currentPitch = euler.x;
            _currentYaw = euler.y;
            PitchRotation.Rotation = Quaternion.Euler(_currentPitch, 0, 0);
            YawRotation.Rotation = Quaternion.Euler(0, _currentYaw, 0);
            // PitchRotation.Rotation = Quaternion.Euler(euler.x, 0, 0);
            // YawRotation.Rotation = Quaternion.Euler(0, euler.y, 0);
        }

        public Vector3 GetRotation()
        {
            return new Vector3(_currentPitch, _currentYaw);
        }
    }
}