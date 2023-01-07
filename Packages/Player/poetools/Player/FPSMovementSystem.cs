using System;
using poetools.Abstraction;
using UnityEngine;

namespace poetools.Player
{
    [Serializable]
    public class FPSMovementSystem
    {
        public float speed = 3;
        public float acceleration = 15;
        public float deceleration = 7;
        public float reactivity = 3;
        public bool useSmoothStop = true;

        public Vector2 InputDirection { get; set; }
        public IRotationComponent YawRotation { get; set; }
        public IPhysicsComponent Physics { get; set; }

        private Vector3 _targetVelocity;
        private Vector3 _currentVelocity;
        private bool _isAccelerating;
    
        public void UpdateMovement()
        {
            _isAccelerating = InputDirection != Vector2.zero;
            _currentVelocity = Physics.Velocity;
        
            CalculateTargetVelocity();
        
            if (_isAccelerating || useSmoothStop == false)
                ApplyConstantAcceleration();

            else if (useSmoothStop)
                ApplySmoothStop();
        }

        private void CalculateTargetVelocity()
        {
            Vector3 forwardVelocity = YawRotation.Forward * InputDirection.y;
            Vector3 rightVelocity = YawRotation.Right * InputDirection.x;
            _targetVelocity = (forwardVelocity + rightVelocity).normalized * speed;
            _targetVelocity.y = _currentVelocity.y;
        }

        private void ApplyConstantAcceleration()
        {
            bool didReverse = _targetVelocity.x != 0 && _currentVelocity.x != 0 &&
                              // ReSharper disable once CompareOfFloatsByEqualityOperator
                              Mathf.Sign(_targetVelocity.x) != Mathf.Sign(_currentVelocity.x);
        
            float currentAcceleration = _isAccelerating ? acceleration : deceleration;

            float reverseMultiplier = didReverse ? reactivity : 1;
            float maxDelta = currentAcceleration * reverseMultiplier * Time.deltaTime;
        
            Physics.Velocity = Vector3.MoveTowards(_currentVelocity, _targetVelocity, maxDelta);
        }

        private void ApplySmoothStop()
        {
            Vector3 smoothedVelocity = _currentVelocity;

            float currentDeceleration = deceleration * Time.deltaTime;
            smoothedVelocity.x -= smoothedVelocity.x * currentDeceleration;
            smoothedVelocity.z -= smoothedVelocity.z * currentDeceleration;

            Physics.Velocity = smoothedVelocity;
        }
    }
}