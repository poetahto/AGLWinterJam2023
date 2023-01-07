using System;
using poetools.Tools;
using UnityEngine;
using UnityEngine.Assertions;

namespace poetools.Abstraction.Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyWrapper : PhysicsComponent
    {
        public Vector3 groundCheckOffset;
        private Rigidbody _rigidbody;
        private bool _isGrounded;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            UpdateIsGrounded();
        }

        public override Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }

        public override bool IsGrounded => _isGrounded;
        public override float AirTime => TimeSpentFalling;
        public override float GroundTime => TimeSpentGrounded;
        
        [SerializeField]
        [Tooltip("The downwards direction used to check if we are grounded.")]
        private Vector3 gravityDirection = Vector3.down;
        
        [SerializeField]
        [Tooltip("How steep a slope we can climb without slipping.")]
        private float slopeLimitDegrees = 45f;
        
        [SerializeField]
        [Tooltip("Draws debug information to the screen.")]
        private bool showDebug;
        
        private void UpdateIsGrounded()
        {
            WasGroundedLastFrame = IsGrounded;
            CheckIsGrounded();
    
            if (JustEntered)
                TimeSpentFalling = 0;
    
            if (JustExited)
                TimeSpentGrounded = 0;
            
            if (IsGrounded)
                TimeSpentGrounded += Time.deltaTime;
            
            else TimeSpentFalling += Time.deltaTime;
        }

        #region Useful Data

            // todo: All of this stuff is useful: its just not required by the interface right now, maybe create a new one? 
            
            private bool WasGroundedLastFrame { get; set; }
            private float TimeSpentGrounded { get; set; }
            private float TimeSpentFalling { get; set; }
            
            private bool JustEntered => IsGrounded && !WasGroundedLastFrame;
            private bool JustExited => !IsGrounded && WasGroundedLastFrame;

            private Vector3 ContactNormal { get; set; }
            private Collider ConnectedCollider { get; set; }

        #endregion

        private Vector3 _previousPosition;
        private Vector3 _currentPosition;
        private Vector3 _velocity;
    
        public float groundDistance = 1;

        private const int MaxHits = 10;
        private RaycastHit[] _hits = new RaycastHit[MaxHits];
        
        private void CheckIsGrounded()
        {
            int hits = Physics.BoxCastNonAlloc(transform.position + groundCheckOffset, new Vector3(0.25f, Mathf.Abs(groundDistance/2), 0.25f) * 0.9f, -transform.up, _hits, Quaternion.identity,groundDistance/2);
            
            Assert.IsTrue(hits <= MaxHits);

            int bestFit = -1;
            float closestDistance = float.PositiveInfinity;

            for (int i = 0; i < hits; i++)
            {
                var cur = _hits[i];

                // We cannot stand on triggers, so early out.
                if (cur.collider.isTrigger || cur.transform == transform)
                    continue;
                
                // We can only stand on slopes with the desired steepness
                Vector3 upDirection = -gravityDirection;
                Vector3 normalDirection = cur.normal;
                float slopeAngle = Vector3.Angle(upDirection, normalDirection);

                // We only want to check the nearest collider we hit.
                if (slopeAngle <= slopeLimitDegrees && cur.distance < closestDistance)
                    bestFit = i;
            }

            if (bestFit >= 0)
            {
                _isGrounded = true;
                ConnectedCollider = _hits[bestFit].collider;
                ContactNormal = _hits[bestFit].normal;
            }
            else
            {
                _isGrounded = false;
                ConnectedCollider = null;
                ContactNormal = Vector3.zero;
            }
            
            DebugDrawTools.Arrow(transform.position + groundCheckOffset, transform.position + groundCheckOffset - (transform.up * groundDistance), _isGrounded ? Color.green : Color.red, 0);
        }

        #region Debug
    
            private void OnGUI()
            {
                if (showDebug)
                {
                    string connectedCollider = ConnectedCollider ? ConnectedCollider.name : "None";
                        
                    GUILayout.Label($"IsGrounded: {IsGrounded}");
                    GUILayout.Label($"Was Grounded Last Frame: {WasGroundedLastFrame}");
                    GUILayout.Label($"Connected Collider: {connectedCollider}");
                    GUILayout.Label($"Contact Normal: {ContactNormal}");
                    GUILayout.Label($"Time spent grounded: {TimeSpentGrounded}");
                    GUILayout.Label($"Time spent falling: {TimeSpentFalling}");
                    GUILayout.Label($"Velocity: {_velocity}");
                }
            }
    
        #endregion
    }
}