using poetools.Abstraction;
using UnityEngine;

namespace poetools.NewDirectory1
{
    public class FPSQuakeMovementSystem : MonoBehaviour
    {
        #region Inspector

            [SerializeField] 
            private bool showDebug;
            
            [Header("References")] 
            
            [SerializeField]
            private Transform lookDirection;
            
            // [SerializeField] 
            // [Tooltip("The rigidbody that our movement force is applied to.")]
            // private Rigidbody targetRigidbody;
        
            [SerializeField] 
            [Tooltip("Used to determine whether we are grounded or not.")]
            private PhysicsComponent physics;

        #endregion

        private void Awake()
        {
            if (physics == null)
                physics = GetComponent<PhysicsComponent>();
        }

        private float _speed;
        private Vector3 _velocity;

        private void Update()
        {
            _speed = CurrentRunningSpeed;
            _velocity = physics.Velocity;
            var newVel = UpdateVelocity();
            newVel.y = 0;
            newVel = Vector3.ClampMagnitude(newVel, trueMax);
            newVel.y = physics.Velocity.y;
            physics.Velocity = newVel;
        }
        
        #region Properties

            public float SpeedMultiplier { get; set; } = 1;
            
            public Vector2 TargetDirection { get; set; }
        
            public float ForwardSpeedMultiplier { get; set; } = 1;

            public float CurrentRunningSpeed
            {
                get
                {
                    Vector3 velocity = physics.Velocity;
                    velocity.y = 0;
                    return velocity.magnitude;
                }
            }

        #endregion
        
        #region Methods

            private Vector3 CalculateForwardSpeedMultiplier(Vector3 targetDirection)
            {
                Vector3 forward = lookDirection.forward;
                float forwardSpeed = Vector3.Dot(targetDirection, forward);
                
                if (forwardSpeed > 0.1f)
                    return forward * (forwardSpeed * (ForwardSpeedMultiplier - 1));

                return Vector3.zero;
            }

        #endregion

        #region Debug

            private void OnGUI()
            {
                if (showDebug)
                {
                    GUILayout.Label($"Current Speed: {CurrentRunningSpeed:F1}");
                    GUILayout.Label($"Forward Speed Multiplier: {ForwardSpeedMultiplier:F1}");
                    GUILayout.Label($"Target Direction: {TargetDirection}");
                    GUILayout.Label($"Grounded: {physics.IsGrounded}");
                    
                    ForwardSpeedMultiplier = 
                        GUILayout.HorizontalSlider(ForwardSpeedMultiplier, 0, 2);
                }
            }

        #endregion
        
        #region Inspector

        [Header("Settings")]
        [SerializeField] public float noFrictionJumpWindow = 0.1f;
        [SerializeField] public float friction = 5.5f;
        [SerializeField] public float airAcceleration = 40f;
        [SerializeField] public float groundAcceleration = 50f;
        [SerializeField] public float maxAirSpeed = 1f;
        [SerializeField] public float maxGroundSpeed = 1f;
        [SerializeField] public float trueMax;

    #endregion
    
    #region Variables

    #endregion

    public Vector3 UpdateVelocity()
    {
        return physics.IsGrounded ? MoveGround() : MoveAir();
    }
    
    #region Methods

        // todo: friction imp here is still subpar - I don't like how it limits max speed    
    
        private Vector3 MoveGround()
        {
            if (_speed != 0 && physics.GroundTime > noFrictionJumpWindow)
            {
                float drop = _speed * friction * Time.deltaTime;
                // float originalY = _velocity.y;
                _velocity *= Mathf.Max(_speed - drop, 0) / _speed;
                // _velocity.y = originalY;
            }

            return Accelerate(groundAcceleration, maxGroundSpeed * SpeedMultiplier);
        }

        private Vector3 MoveAir()
        {
            return Accelerate(airAcceleration, maxAirSpeed * SpeedMultiplier);
        }
        
        private Vector3 Accelerate(float acceleration, float maxVelocity)
        {
            var targetForward = lookDirection.forward * TargetDirection.y;
            var targetStrafe = lookDirection.right * TargetDirection.x;
            var targetDir = (targetForward + targetStrafe).normalized;
            
            float projVel = Vector3.Dot(_velocity, targetDir);
            float accelVel = acceleration * Time.deltaTime;

            if (projVel + accelVel > maxVelocity)
                accelVel = Mathf.Max(maxVelocity - projVel, 0);
                // accelVel = maxVelocity - projVel;

            return _velocity + targetDir * accelVel;
        }

    #endregion
    }
}